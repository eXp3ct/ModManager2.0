using Expect.ModManager.CurseClient;
using Expect.ModManager.Domain.Configurations;
using Expect.ModManager.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.View
{
	public static class Startup
	{
		public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<CurseClientOptions>(configuration.GetSection(nameof(CurseClientOptions)));
			services.AddInfrastructure();
			services.AddCurseClient();
		}
	}
}
