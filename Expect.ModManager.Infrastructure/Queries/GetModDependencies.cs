using Expect.ModManager.CurseApiClient.Deserialization.Interfaces;
using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Infrastructure.Queries
{
	public class GetModDependenciesQuery : IRequest<IEnumerable<IMod>>
	{
		public Mod Mod { get; set; }

		public GetModDependenciesQuery(Mod mod)
		{
			Mod = mod;
		}
	}

	public class GetModDependenciesQueryHandler : IRequestHandler<GetModDependenciesQuery, IEnumerable<IMod>>
	{
		private readonly IModFileDeserializer _fileDeserializer;
		private readonly IModDeserializer _modDeserilizer;
		private readonly ViewState _viewState;
		private readonly ILogger<GetModDependenciesQueryHandler> _logger;

		public GetModDependenciesQueryHandler(IModFileDeserializer fileDeserializer, ViewState viewState, 
			IModDeserializer modDeserilizer, ILogger<GetModDependenciesQueryHandler> logger)
		{
			_fileDeserializer = fileDeserializer;
			_viewState = viewState;
			_modDeserilizer = modDeserilizer;
			_logger = logger;
		}

		public async Task<IEnumerable<IMod>> Handle(GetModDependenciesQuery request, CancellationToken cancellationToken)
		{
			var modFiles = await _fileDeserializer.GetModFiles(request.Mod.Id, _viewState, 0, 50);

			if(!modFiles.SelectMany(x => x.Dependencies).Any())
			{
				_logger.LogWarning($"Gained 0 dependencies for {request.Mod.Name}");
				return Enumerable.Empty<IMod>();
			}

			var latestFile = modFiles
				.Where(file => file.IsAvailable && file.FileStatus == ModFileStatus.Approved)?
				.OrderByDescending(file => file.FileDate)
				.FirstOrDefault();
			
			var depIds = latestFile?.Dependencies
				.Where(dep => dep.RelationType == ModFileReleationType.RequiredDependency)
				.Select(dep => dep.ModId);

			if (depIds == null || !depIds.Any())
			{
				_logger.LogWarning($"Gained 0 required dependencies for {request.Mod.Name}");
				return Enumerable.Empty<IMod>();
			}

			var depMods = await _modDeserilizer.GetModelsList(depIds);

			_logger.LogInformation($"Gained dependencies for {request.Mod.Name}: {string.Join(" | ", depMods.Select(x => x.Name))}");

			return depMods;
		}
	}
}
