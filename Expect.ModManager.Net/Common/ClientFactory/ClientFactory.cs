using Expect.ModManager.Net.Common.Interfaces;

namespace Expect.ModManager.Net.Common.ClientFactory
{
	public class ClientFactory<TClient> : IClientFactory<TClient> where TClient : HttpClient, IClient
	{
		private readonly Func<TClient> _factory;

		public ClientFactory(Func<TClient> factory)
		{
			_factory = factory;
		}

		public TClient CreateClient()
		{
			var client = _factory();

			client.ApplyAuth();

			return client;
		}
	}
}