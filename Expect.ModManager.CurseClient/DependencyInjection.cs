using Expect.ModManager.CurseApiClient.Deserialization;
using Expect.ModManager.CurseApiClient.Deserialization.Interfaces;
using Expect.ModManager.CurseApiClient.Fetching;
using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.CurseApiClient.Urls;
using Expect.ModManager.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.ModManager.CurseApiClient
{
	public static class DependencyInjection
	{
		public static void AddCurseClient(this IServiceCollection services)
		{
			services.AddSingleton<IGetEndpoint, CurseUris>();

			services.AddScoped<IFetchModString, ModStringFetcher>();
			services.AddScoped<IFetchModFileString, ModFilesStringFetcher>();
			services.AddSingleton<IFetchFeaturesString, FeaturesStringFetcher>();

			services.AddScoped<IModDeserializer, ModDeserializer>();
			services.AddScoped<IModFileDeserializer, ModFileDeserializer>();
			services.AddScoped<IFeaturesDeserizlier, FeaturesDeserializer>();
		}
	}
}
