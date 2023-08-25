using Expect.ModManager.Net.Common;
using Expect.ModManager.Net.Common.ClientFactory;
using Expect.ModManager.Net.Common.Clients;
using Expect.ModManager.Net.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.ModManager.Net
{
	public static class DependencyInjection
	{
		public static void AddHttpClients(this IServiceCollection services)
		{
			services.AddClient<CurseClient>();
		}

		private static void AddClient<TClient>(this IServiceCollection services) where TClient : HttpClient, IClient
		{
			services.AddSingleton<TClient>();
			services.AddSingleton<Func<TClient>>(x => () => x.GetRequiredService<TClient>());
			services.AddSingleton<IClientFactory<TClient>, ClientFactory<TClient>>();
			services.AddSingleton<HttpClient<TClient>>();
		}
	}
}
