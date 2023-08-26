using Expect.ModManager.CurseApiClient.Deserialization.Interfaces;
using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using MediatR;
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

		public GetModDependenciesQueryHandler(IModFileDeserializer fileDeserializer, ViewState viewState, IModDeserializer modDeserilizer)
		{
			_fileDeserializer = fileDeserializer;
			_viewState = viewState;
			_modDeserilizer = modDeserilizer;
		}

		public async Task<IEnumerable<IMod>> Handle(GetModDependenciesQuery request, CancellationToken cancellationToken)
		{
			var modFiles = await _fileDeserializer.GetModFiles(request.Mod.Id, _viewState, 0, 50);

			if(!modFiles.SelectMany(x => x.Dependencies).Any())
			{
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
				return Enumerable.Empty<IMod>();
			}

			var depMods = await _modDeserilizer.GetModelsList(depIds);

			return depMods;
		}
	}
}
