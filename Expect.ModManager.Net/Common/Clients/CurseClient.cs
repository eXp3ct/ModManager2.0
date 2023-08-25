using Expect.ModManager.Domain.Configurations;
using Expect.ModManager.Net.Common.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Expect.ModManager.Net.Common.Clients
{
	public class CurseClient : HttpClient, IClient
	{
		private readonly IOptions<CurseClientOptions> _options;

		public CurseClient(IOptions<CurseClientOptions> options)
		{
			_options = options;
		}

		public void ApplyAuth()
		{
			DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			DefaultRequestHeaders.Add("x-api-key", _options.Value.ApiKey);
		}
	}
}