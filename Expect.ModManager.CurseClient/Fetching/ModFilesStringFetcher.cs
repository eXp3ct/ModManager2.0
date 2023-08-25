using Expect.ModManager.CurseApiClient.Extensions;
using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.CurseApiClient.Urls;
using Expect.ModManager.CurseApiClient.Urls.Enums;
using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Net.Common;
using Expect.ModManager.Net.Common.Clients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.CurseApiClient.Fetching
{
	public class ModFilesStringFetcher : IFetchModFileString
	{
		private readonly HttpClient<CurseClient> _client;
		private readonly IGetEndpoint _endpoint;

		public ModFilesStringFetcher(HttpClient<CurseClient> client, IGetEndpoint endpoint)
		{
			_client = client;
			_endpoint = endpoint;
		}

		public async Task<string> GetList(IEnumerable<int> fileIds)
		{
			var url = _endpoint.GetEndpoint(RequestType.GetFiles);
			var requestBody = JsonConvert.SerializeObject(new { fileIds });

			using var request = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
			};

			using var response = await _client.Client.SendAsync(request);

			return await response.TryReturnString();
		}

		public async Task<string> GetModFile(int modId, int fileId)
		{
			var url = _endpoint.GetEndpoint(RequestType.GetModFile, modId, fileId);

			using var response = await _client.Client.GetAsync(url);

			return await response.TryReturnString();
		}

		public async Task<string> GetDownloadUrl(int modId, int fileId)
		{
			var url = _endpoint.GetEndpoint(RequestType.GetModFileDownloadUrl, modId, fileId);

			using var response = await _client.Client.GetAsync(url);

			return await response.TryReturnString();
		}

		public async Task<string> GetModFiles(int modId, 
			string gameVersion = null, 
			ModLoaderType modLoaderType = ModLoaderType.Any, int index = 0, int pageSize = 0)
		{
			var queryParams = new Dictionary<string, string>
			{
				{ "gameVersion", gameVersion },
				{ "modLoaderType", ((int)modLoaderType).ToString() },
				{ "index", index.ToString() },
				{ "pageSize", pageSize.ToString() }
			};

			var url = default(string);

			if (queryParams.Any())
			{
				url = $"{_endpoint.GetEndpoint(RequestType.GetModFiles, modId)}?{string.Join('&', queryParams
					.Select(q => $"{q.Key}={q.Value}"))}";
			}

			using var response = await _client.Client.GetAsync(url);

			return await response.TryReturnString();
		}
	}
}
