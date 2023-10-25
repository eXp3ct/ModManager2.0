using Expect.ModManager.CurseApiClient.Deserialization.Interfaces;
using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Infrastructure.Events;
using Expect.ModManager.Net.Common.Clients;
using Expect.ModManager.Net.Downloading;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Expect.ModManager.Infrastructure.Queries
{
	public class InstallModsQuery : IRequest<IEnumerable<KeyValuePair<Mod, ModFile>>?>
	{
		public event EventHandler<ReportEventArgs> OnReport;

		public void Report(double value)
		{
			OnReport?.Invoke(this, new ReportEventArgs(value));
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
            var timer = new Stopwatch();
            timer.Start();
            var errorResult = new List<KeyValuePair<Mod, ModFile>>();
            var fileQueue = new ConcurrentDictionary<Mod, ModFile>();
            var mods = _selectedMods;

            if (mods == null)
                return null;

            // Создаем список задач для параллельной обработки модов
            var downloadTasks = mods.AsParallel().Select(async mod =>
            {
                var modFiles = await _fileDeserializer.GetModFiles(mod.Id, _viewState, 0, 50);

                if (modFiles == null || !modFiles.Any())
                    return;

                var latestFile = modFiles
                    .Where(file => (file.ReleaseType == ModFileReleaseType.Release || file.ReleaseType == ModFileReleaseType.Beta) && file.IsAvailable)
                    .OrderByDescending(file => file.FileDate)
                    .FirstOrDefault();

                if (latestFile == null)
                    return;

                var depModsPairs = await GetDependencyModModFilePairs(mod);

                foreach (var depMod in depModsPairs)
                {
                    if (fileQueue.ContainsKey(depMod.Key))
                        continue;
                    fileQueue.TryAdd(depMod.Key, depMod.Value);
                }

                var pair = new KeyValuePair<Mod, ModFile>(mod, latestFile);
                if (fileQueue.ContainsKey(pair.Key))
                    return;

                fileQueue.TryAdd(pair.Key, pair.Value);
            });

            await Task.WhenAll(downloadTasks);

            _logger.LogInformation($"Mods to install: {string.Join(" | ", fileQueue.Keys.Select(x => x.Name).Distinct())}");

            var unique = fileQueue.DistinctBy(pair => pair.Key.Id);

            var totalLength = unique
                .Select(x => x.Value.FileLength)
                .Sum();

            // Создаем список задач для параллельной установки модов
            var installTasks = unique.Select(async (modFilePair) =>
            {
                _logger.LogInformation($"Started installing {modFilePair.Key.Name}");
                var downloadUrl = await _fileDeserializer.GetDownloadUrl(modFilePair.Key.Id, modFilePair.Value.Id);

                if (downloadUrl == null)
                {
                    errorResult.Add(modFilePair);
                    _logger.LogError($"Cannot install {modFilePair.Key.Name}, download URL was null");
                    return;
                }

                var fileBytes = await _downloader.Download(downloadUrl);

                using var fileStream = new FileStream($"{_viewState.FolderPath}/{modFilePair.Value.FileName}", FileMode.Create);

                var value = modFilePair.Value.FileLength / (double)totalLength;
                value *= 100;
                request.Report(value);

                fileStream.Write(fileBytes, 0, fileBytes.Length);
                _logger.LogInformation($"Installed {modFilePair.Key.Name}");
            });

            await Task.WhenAll(installTasks);

            _logger.LogInformation($"Done installing {unique.Count()} mods");
            timer.Stop();
            _logger.LogDebug("Time for downloading {count} mods is: {time}", unique.Count(), timer.Elapsed);
            return errorResult;
        }

        private async Task<IEnumerable<Mod>> GetDependencyMod(Mod mod)
		{
			var query = new GetModDependenciesQuery(mod);

			var depMods = await _mediator.Send(query);

			if (!depMods.Any())
				return Enumerable.Empty<Mod>();

			return (IEnumerable<Mod>)depMods;
		}

		private async Task<IEnumerable<KeyValuePair<Mod, ModFile>>> GetDependencyModModFilePairs(Mod mod)
		{
			var depMods = await GetDependencyMod(mod);
			var set = new HashSet<KeyValuePair<Mod, ModFile>>();
			
			foreach (var depMod in depMods)
			{
				var modFiles = await _fileDeserializer.GetModFiles(depMod.Id, _viewState, 0, 20);

				if (modFiles == null || !modFiles.Any())
					continue;

				var latestModFile = modFiles
					.OrderByDescending(file => file.FileDate)
					.FirstOrDefault();

				if (latestModFile.Dependencies.Any(x => x.RelationType == ModFileReleationType.RequiredDependency))
				{
					foreach (var innerDep in await GetDependencyModModFilePairs(depMod))
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
