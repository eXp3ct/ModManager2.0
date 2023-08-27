using Expect.ModManager.CurseApiClient.Exceptions;
using Expect.ModManager.CurseApiClient.Extensions;
using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.CurseApiClient.Urls;
using Expect.ModManager.CurseApiClient.Urls.Enums;
using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Net.Common;
using Expect.ModManager.Net.Common.Clients;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Expect.ModManager.CurseApiClient.Fetching
{
	public class ModFilesStringFetcher : IFetchModFileString
	{
		private readonly HttpClient<CurseClient> _client;
		private readonly IGetEndpoint _endpoint;
		private readonly ILogger<ModFilesStringFetcher> _logger;

		public ModFilesStringFetcher(HttpClient<CurseClient> client, IGetEndpoint endpoint, ILogger<ModFilesStringFetcher> logger)
		{
			_client = client;
			_endpoint = endpoint;
			_logger = logger;
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

			var result = await response.TryReturnString();

			if (string.IsNullOrEmpty(result))
			{
				_logger.LogError(new FetchingException(response.StatusCode, response.RequestMessage?.RequestUri), $"Cannot fetch json for mod files {string.Join(" | ", fileIds)}");
				return result;
			}

			_logger.LogInformation($"Fetchted json for mod files {string.Join(" | ", fileIds)}");
			return result;
		}

		public async Task<string> GetModFile(int modId, int fileId)
		{
			var url = _endpoint.GetEndpoint(RequestType.GetModFile, modId, fileId);

			using var response = await _client.Client.GetAsync(url);

			var result = await response.TryReturnString();
			if (string.IsNullOrEmpty(result))
			{
				_logger.LogError(new FetchingException(response.StatusCode, response.RequestMessage?.RequestUri), $"Cannot fetch json for mod {modId} file {fileId}");
				return result;
			}

			_logger.LogInformation($"Fetchted json for mod {modId} file {fileId}");

			return result;
		}

		public async Task<string?> GetDownloadUrl(int modId, int fileId)
		{
			var url = _endpoint.GetEndpoint(RequestType.GetModFileDownloadUrl, modId, fileId);

			using var response = await _client.Client.GetAsync(url);

			var result = await response.TryReturnString();

			if (string.IsNullOrEmpty(result))
			{
				_logger.LogError(new FetchingException(response.StatusCode, response.RequestMessage?.RequestUri), $"Cannot fetch json for download url mod {modId} file {fileId}");
				return result;
			}

			_logger.LogInformation($"Fetchted json for download url mod {modId} file {fileId}");

			return result;
		}

		public async Task<string> GetModFiles(int modId,
			string gameVersion = "",
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
					.Where(pair => !string.IsNullOrEmpty(pair.Value))
					.Select(q => $"{q.Key}={q.Value}"))}";
			}

			using var response = await _client.Client.GetAsync(url);

			var result = await response.TryReturnString();

			if (string.IsNullOrEmpty(result))
			{
				_logger.LogError(new FetchingException(response.StatusCode, response.RequestMessage?.RequestUri), $"Cannot fetch json for mod {modId} files");
				return result;
			}

			_logger.LogInformation($"Fetchted json for mod {modId} files");

			return result;
		}
	}
}
