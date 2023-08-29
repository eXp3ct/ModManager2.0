using Expect.ModManager.CurseApiClient.Deserialization.Interfaces;
using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Infrastructure.Events;
using Expect.ModManager.Net.Common.Clients;
using Expect.ModManager.Net.Downloading;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Infrastructure.Queries
{
	public class InstallModsQuery : IRequest<IEnumerable<KeyValuePair<Mod, ModFile>>?>
	{
		public event EventHandler<ReportEventArgs> Report;

		public void OnReport(double value)
		{
			Report?.Invoke(this, new ReportEventArgs(value));
		}
	}

	public class InstallModsQueryHandler : IRequestHandler<InstallModsQuery, IEnumerable<KeyValuePair<Mod, ModFile>>?>
	{
		private readonly IDownloader<CurseClient> _downloader;
		private readonly ILogger<InstallModsQueryHandler> _logger;
		private readonly IModFileDeserializer _fileDeserializer;
		private readonly IList<Mod> _selectedMods;
		private readonly ViewState _viewState;
		private readonly IMediator _mediator;
		

		public InstallModsQueryHandler(IDownloader<CurseClient> downloader,
			ILogger<InstallModsQueryHandler> logger,
			IModFileDeserializer fileDeserializer,
			IList<Mod> selectedModsIds,
			ViewState viewState,
			IMediator mediator)
		{
			_downloader = downloader;
			_logger = logger;
			_fileDeserializer = fileDeserializer;
			_selectedMods = selectedModsIds;
			_viewState = viewState;
			_mediator = mediator;
		}

		public async Task<IEnumerable<KeyValuePair<Mod, ModFile>>?> Handle(InstallModsQuery request, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Started downloading {_selectedMods.Count}");
			var errorResult = new List<KeyValuePair<Mod, ModFile>>();

			var mods = _selectedMods;

			var fileQueue = new Dictionary<Mod, ModFile>();

			if (mods == null)
				return null;

			foreach(var mod in mods)
			{
				var modFiles = await _fileDeserializer.GetModFiles(mod.Id, _viewState, 0, 50);

				if (modFiles == null || !modFiles.Any())
					continue;

				var latestFile = modFiles
					.Where(file => (file.ReleaseType == ModFileReleaseType.Release || file.ReleaseType == ModFileReleaseType.Beta ) && file.IsAvailable)
					.OrderByDescending(file => file.FileDate)
					.FirstOrDefault();

				var depModsPairs = await GetDependencyModModFilePairs(mod);

				foreach (var depMod in depModsPairs)
				{
					if (fileQueue.ContainsKey(depMod.Key))
						continue;
					fileQueue.Add(depMod.Key, depMod.Value);
				}
					

				var pair = new KeyValuePair<Mod, ModFile>(mod, latestFile);
				if (fileQueue.ContainsKey(pair.Key))
					continue;
				fileQueue.Add(pair.Key, pair.Value);
			}

			_logger.LogInformation($"Mods to install: {string.Join(" | ", fileQueue.Keys.Select(x => x.Name).Distinct())}");

			var unique = fileQueue.DistinctBy(pair => pair.Key.Id);

			var totalLength = unique
				.Select(x => x.Value.FileLength)
				.Sum();

			foreach (var (mod, file) in unique)
			{
				_logger.LogInformation($"Started installing {mod.Name}");
				var downloadUrl = await _fileDeserializer.GetDownloadUrl(mod.Id, file.Id);

				if(downloadUrl == null)
				{
					errorResult.Add(new KeyValuePair<Mod, ModFile>(mod, file));
					_logger.LogError($"Cannot install {mod.Name}, download url was null");
					continue;
				}

				var fileBytes = await _downloader.Download(downloadUrl);

				using var fileStream = new FileStream($"{_viewState.FolderPath}/{file.FileName}", FileMode.Create);

				var value = file.FileLength / (double)totalLength ;
				value *= 100;
				request.OnReport(value);

				fileStream.Write(fileBytes, 0, fileBytes.Length);
				_logger.LogInformation($"Installed {mod.Name}");
			}

			_logger.LogInformation($"Done installing {unique.Count()} mods");

			return errorResult;
		}

		private async Task<IEnumerable<Mod>> GetDependencyMod(Mod mod)
		{
			var query = new GetModDependenciesQuery(mod);

			var depMods = await _mediator.Send(query);
			
			if(!depMods.Any())
				return Enumerable.Empty<Mod>();

			return (IEnumerable<Mod>)depMods;
		}

		private async Task<IEnumerable<KeyValuePair<Mod, ModFile>>> GetDependencyModModFilePairs(Mod mod)
		{
			var depMods = await GetDependencyMod(mod);
			var set = new HashSet<KeyValuePair<Mod,ModFile>>();

			foreach(var depMod in depMods)
			{
				var modFiles = await _fileDeserializer.GetModFiles(depMod.Id, _viewState, 0, 20);

				if (modFiles == null || !modFiles.Any())
					continue;

				var latestModFile = modFiles
					.OrderByDescending(file => file.FileDate)
					.FirstOrDefault();

				if(latestModFile.Dependencies.Any(x => x.RelationType == ModFileReleationType.RequiredDependency))
				{
					foreach(var innerDep in await GetDependencyModModFilePairs(depMod))
					{
						set.Add(innerDep);
					}
				}

				set.Add(new KeyValuePair<Mod, ModFile>(depMod, latestModFile));
				
			}

			return set.Distinct();
		}
	}
}
