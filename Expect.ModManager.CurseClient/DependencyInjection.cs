using Expect.ModManager.CurseApiClient.Fetching;
using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.CurseApiClient.Urls;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.ModManager.CurseApiClient
{
	public static class DependencyInjection
	{
		public static void AddCurseClient(this IServiceCollection services)
		{
			services.AddSingleton<IGetEndpoint, CurseUris>();
			services.AddScoped<IGetModsResponse, GetModsResponse>();
			services.AddScoped<IGetModFilesResponse, GetModFilesResponse>();
			services.AddSingleton<IGetFeaturesResponse, GetFeaturesResponse>();
		}
	}
}
