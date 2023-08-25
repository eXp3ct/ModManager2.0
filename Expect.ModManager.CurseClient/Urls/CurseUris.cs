using Expect.ModManager.CurseApiClient.Urls.Enums;
using Expect.ModManager.Domain.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.CurseApiClient.Urls
{
	public class CurseUris : IGetEndpoint
	{
		private readonly IOptions<CurseClientOptions> _options;
		public CurseUris(IOptions<CurseClientOptions> options)
		{
			_options = options;
		}

		public Uri GetEndpoint(RequestType requestType, int? modId = null, int? fileId = null)
		{
			var endpoints = GetEndpoints();

			if (endpoints.TryGetValue(requestType, out var endpoint))
			{
				if (modId != null && endpoint.Contains("{modId}"))
				{
					endpoint = endpoint.Replace("{modId}", modId.ToString());
				}
				if (fileId != null && endpoint.Contains("{fileId}"))
				{
					endpoint = endpoint.Replace("{fileId}", fileId.ToString());
				}

				return new(endpoint);
			}
			else
			{
				throw new ArgumentException($"Invalid request type: {requestType}");
			}
		}

		private Dictionary<RequestType, string> GetEndpoints()
		{
			return new()
			{
				{RequestType.SearchMod, _options.Value.Endpoints.SearchMods },
				{RequestType.GetMod, _options.Value.Endpoints.GetMod },
				{RequestType.GetMods, _options.Value.Endpoints.GetMods },
				{RequestType.GetModDescription, _options.Value.Endpoints.GetModDescription },
				{RequestType.GetCategories, _options.Value.Endpoints.GetCategories },
				{RequestType.GetMinecraftVersions, _options.Value.Endpoints.GetMinecraftVersions },
				{RequestType.GetMinecraftModLoaders, _options.Value.Endpoints.GetMinecraftModLoaders },
				{RequestType.GetModFile, _options.Value.Endpoints.GetModFile },
				{RequestType.GetModFiles, _options.Value.Endpoints.GetModFiles },
				{RequestType.GetFiles, _options.Value.Endpoints.GetFiles },
				{RequestType.GetModFileDownloadUrl, _options.Value.Endpoints.GetModFileDownloadUrl },
			};
		}
	}
}
