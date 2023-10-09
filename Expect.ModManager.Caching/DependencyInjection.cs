using Expect.ModManager.Caching.Interfaces;
using Expect.ModManager.Caching.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.ModManager.Caching
{
	public static class DependencyInjection
	{
		public static void AddInMemoryCaching(this IServiceCollection services)
		{
			services.AddMemoryCache();

			services.AddSingleton<IModProvider, ModProvider>();
		}
	}
}
