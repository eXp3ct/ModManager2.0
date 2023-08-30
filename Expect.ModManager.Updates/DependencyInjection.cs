using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Updates
{
	public static class DependencyInjection
	{
		public static void AddAutoUpdates(this IServiceCollection services)
		{
			services.AddSingleton<Updater>();
		}
	}
}
