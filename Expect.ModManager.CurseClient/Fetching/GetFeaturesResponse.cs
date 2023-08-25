using Expect.ModManager.CurseApiClient.Extensions;
using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.CurseApiClient.Urls;
using Expect.ModManager.CurseApiClient.Urls.Enums;
using Expect.ModManager.Net.Common;
using Expect.ModManager.Net.Common.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.CurseApiClient.Fetching
{
	public class GetFeaturesResponse : IGetFeaturesResponse
	{
		private readonly HttpClient<CurseClient> _client;
		private readonly IGetEndpoint _endpoint;

		public GetFeaturesResponse(HttpClient<CurseClient> client, IGetEndpoint endpoint)
		{
			_client = client;
			_endpoint = endpoint;
		}

		public async Task<string> GetCategories(int gameId, int classId = 0)
		{
			var url = $"{_endpoint.GetEndpoint(RequestType.GetCategories)}?gameId={gameId}&classId={classId}";

			using var response = await _client.Client.GetAsync(url);

			return await response.TryReturnString();
		}

		public async Task<string> GetMinecraftModLoaders(string version = null, bool includeAll = false)
		{
			var url = $"{_endpoint.GetEndpoint(RequestType.GetMinecraftModLoaders)}?version={version}&includeAll={includeAll}";

			using var response = await _client.Client.GetAsync(url);

			return await response.TryReturnString();
		}

		public async Task<string> GetMinecraftVersions(bool sortDescending = false)
		{
			var url = $"{_endpoint.GetEndpoint(RequestType.GetMinecraftVersions)}?sortDescending={sortDescending}";

			using var response = await _client.Client.GetAsync(url);

			return await response.TryReturnString();
		}
	}
}
