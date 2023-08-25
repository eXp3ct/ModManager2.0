using Expect.ModManager.CurseClient.Common.Interfaces;
using Expect.ModManager.Domain.Configurations;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Expect.ModManager.CurseClient.Common
{
	public class Client : HttpClient, IClient
	{
		private readonly IOptions<CurseClientOptions> _options;
		public Client(IOptions<CurseClientOptions> options)
		{
			_options = options;

			ConfigureClient();
		}

		private void ConfigureClient()
		{
			DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			DefaultRequestHeaders.Add("x-api-key", _options.Value.ApiKey);
		}
	}
}
