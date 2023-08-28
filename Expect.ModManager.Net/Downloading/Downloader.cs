using Expect.ModManager.Net.Common;
using Expect.ModManager.Net.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Net.Downloading
{
	public class Downloader<TClient> : IDownloader<TClient> where TClient : HttpClient, IClient
	{
		private readonly HttpClient _client;
		private readonly ILogger<Downloader<TClient>> _logger;

		public Downloader(HttpClient<TClient> client, ILogger<Downloader<TClient>> logger)
		{
			_client = client.Client;
			_logger = logger;
		}

		public async Task<byte[]> Download(string url)
		{
			if(string.IsNullOrEmpty(url))
				return Array.Empty<byte>();

			using var response = await _client.GetAsync(url);

			if(response.IsSuccessStatusCode)
			{
				_logger.LogInformation($"Gained file's byte array");
				return await response.Content.ReadAsByteArrayAsync();
			}

			_logger.LogError($"Cannot gain file's byte array");
			return Array.Empty<byte>();
		}
	}
}
