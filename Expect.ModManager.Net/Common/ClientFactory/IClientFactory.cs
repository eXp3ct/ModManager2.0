using Expect.ModManager.Net.Common.Interfaces;

namespace Expect.ModManager.Net.Common.ClientFactory
{
	public interface IClientFactory<TClient> where TClient : HttpClient, IClient
	{
		public TClient CreateClient();
	}
}