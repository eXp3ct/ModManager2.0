using Expect.ModManager.CurseApiClient;
using Expect.ModManager.Domain.Configurations;
using Expect.ModManager.Infrastructure;
using Expect.ModManager.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.ModManager.View
{
	public static class Startup
	{
		public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<CurseClientOptions>(configuration.GetSection(nameof(CurseClientOptions)));

			services.AddInfrastructure();
			services.AddCurseClient();
			services.AddHttpClients();
		}
	}
}
