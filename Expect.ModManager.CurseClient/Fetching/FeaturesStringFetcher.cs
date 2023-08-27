using Expect.ModManager.CurseApiClient.Exceptions;
using Expect.ModManager.CurseApiClient.Extensions;
using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.CurseApiClient.Urls;
using Expect.ModManager.CurseApiClient.Urls.Enums;
using Expect.ModManager.Net.Common;
using Expect.ModManager.Net.Common.Clients;
using Microsoft.Extensions.Logging;

namespace Expect.ModManager.CurseApiClient.Fetching
{
	public class FeaturesStringFetcher : IFetchFeaturesString
	{
		private readonly HttpClient<CurseClient> _client;
		private readonly IGetEndpoint _endpoint;
		private readonly ILogger<FeaturesStringFetcher> _logger;

		public FeaturesStringFetcher(HttpClient<CurseClient> client, IGetEndpoint endpoint, ILogger<FeaturesStringFetcher> logger)
		{
			_client = client;
			_endpoint = endpoint;
			_logger = logger;
		}

		public async Task<string> GetCategories(int gameId, int classId = 0)
		{
			var url = $"{_endpoint.GetEndpoint(RequestType.GetCategories)}?gameId={gameId}&classId={classId}";

			using var response = await _client.Client.GetAsync(url);

			var result = await response.TryReturnString();

			if (string.IsNullOrEmpty(result))
			{
				_logger.LogError(new FetchingException(response.StatusCode, response.RequestMessage?.RequestUri), $"Cannot fetch json for categories");
				return result;
			}

			_logger.LogInformation("Fetchted json for categories");

			return result;
		}

		public async Task<string> GetMinecraftModLoaders(string version = null, bool includeAll = false)
		{
			var url = $"{_endpoint.GetEndpoint(RequestType.GetMinecraftModLoaders)}?version={version}&includeAll={includeAll}";

			using var response = await _client.Client.GetAsync(url);

			var result = await response.TryReturnString();

			if (string.IsNullOrEmpty(result))
			{
				_logger.LogError(new FetchingException(response.StatusCode, response.RequestMessage?.RequestUri), $"Cannot fetch json for minecraft mod loaders {version}");
				return result;
			}

			_logger.LogInformation($"Fetched json for minecraft mod loaders {version}");

			return result;
		}

		public async Task<string> GetMinecraftVersions(bool sortDescending = false)
		{
			var url = $"{_endpoint.GetEndpoint(RequestType.GetMinecraftVersions)}?sortDescending={sortDescending}";

			using var response = await _client.Client.GetAsync(url);

			var result = await response.TryReturnString();

			if (string.IsNullOrEmpty(result))
			{
				_logger.LogError(new FetchingException(response.StatusCode, response.RequestMessage?.RequestUri), $"Cannot fetch json for minecraft versions");
				return result;
			}

			_logger.LogInformation("Fetchted json for minecraft versions");

			return result;
		}
	}
}
