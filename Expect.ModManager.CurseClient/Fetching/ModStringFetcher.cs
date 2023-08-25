using Expect.ModManager.CurseApiClient.Extensions;
using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.CurseApiClient.Urls;
using Expect.ModManager.CurseApiClient.Urls.Enums;
using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Net.Common;
using Expect.ModManager.Net.Common.Clients;
using Newtonsoft.Json;
using System.Text;

namespace Expect.ModManager.CurseApiClient.Fetching
{
	public class ModStringFetcher : IFetchModString
	{
		private readonly HttpClient<CurseClient> _client;
		private readonly IGetEndpoint _endpoint;

		public ModStringFetcher(HttpClient<CurseClient> client, IGetEndpoint endpoint)
		{
			_client = client;
			_endpoint = endpoint;
		}

		public async Task<string> GetMod(int modId)
		{
			using var response = await _client.Client.GetAsync(_endpoint.GetEndpoint(RequestType.GetMod, modId));

			return await response.TryReturnString();
		}

		public async Task<string> GetModDescription(int modId)
		{
			using var response = await _client.Client.GetAsync(_endpoint.GetEndpoint(RequestType.GetModDescription, modId));

			return await response.TryReturnString();
		}

		public async Task<string> GetList(IEnumerable<int> modIds)
		{
			var url = _endpoint.GetEndpoint(RequestType.GetMods);
			var requestBody = JsonConvert.SerializeObject(new { modIds });

			using var request = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
			};

			using var response = await _client.Client.SendAsync(request);

			return await response.TryReturnString();
		}

		public async Task<string> SearchMods(
			int gameId,
			int classId = 0, int categoryId = 0,
			string gameVersion = "", string searchFilter = "",
			SearchSortFields sortField = SearchSortFields.LastUpdated, string sortOrder = "asc",
			ModLoaderType modLoaderType = ModLoaderType.Any,
			int gameVersionTypeId = 0, int authorId = 0,
			string slug = "", int index = 0,
			int pageSize = 0)
		{
			var queryString = new StringBuilder();
			queryString.Append($"gameId={gameId}");
			if (classId != 0) queryString.Append($"&classId={classId}");
			if (categoryId != 0) queryString.Append($"&categoryId={categoryId}");
			if (!string.IsNullOrEmpty(gameVersion)) queryString.Append($"&gameVersion={Uri.EscapeDataString(gameVersion)}");
			if (!string.IsNullOrEmpty(searchFilter)) queryString.Append($"&searchFilter={Uri.EscapeDataString(searchFilter)}");
			queryString.Append($"&sortField={(int)sortField}");
			queryString.Append($"&sortOrder={sortOrder}");
			if(!string.IsNullOrEmpty(gameVersion)) queryString.Append($"&modLoaderType={(int)modLoaderType}");
			if (gameVersionTypeId != 0) queryString.Append($"&gameVersionTypeId={gameVersionTypeId}");
			if (authorId != 0) queryString.Append($"&authorId={authorId}");
			if (!string.IsNullOrEmpty(slug)) queryString.Append($"&slug={Uri.EscapeDataString(slug)}");
			queryString.Append($"&index={index}");
			queryString.Append($"&pageSize={pageSize}");

			var url = $"{_endpoint.GetEndpoint(RequestType.SearchMod)}?{queryString}";

			using var response = await _client.Client.GetAsync(url);

			return await response.TryReturnString();
		}

	}
}
