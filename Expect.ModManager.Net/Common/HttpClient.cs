using Expect.ModManager.Net.Common.ClientFactory;
using Expect.ModManager.Net.Common.Interfaces;

namespace Expect.ModManager.Net.Common
{
	public class HttpClient<TClient> where TClient : HttpClient, IClient
	{
		public HttpClient Client { get; set; }

		public HttpClient(IClientFactory<TClient> clientFactory)
		{
			Client = clientFactory.CreateClient();
		}
	}
}
