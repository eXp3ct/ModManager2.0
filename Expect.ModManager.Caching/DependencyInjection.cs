using Expect.ModManager.Caching.Interfaces;
using Expect.ModManager.Caching.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Caching
{
	public static class DependencyInjection
	{
		public static void AddInMemoryCaching(this IServiceCollection services) 
		{
			services.AddMemoryCache();

			services.AddSingleton(new MemoryCacheEntryOptions()
			{
				SlidingExpiration = TimeSpan.FromHours(1),
				AbsoluteExpiration = new DateTimeOffset(DateTime.Now, TimeSpan.FromHours(3))
			});

			services.AddSingleton<IModProvider, ModProvider>();
		}
	}
}
