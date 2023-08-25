using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.CurseApiClient.Urls.Enums
{
	public enum RequestType
	{
		SearchMod = 1,
		GetMod = 2,
		GetMods = 3,
		GetModDescription = 4,
		GetCategories = 5,
		GetMinecraftVersions = 6,
		GetMinecraftModLoaders = 7,
		GetModFile = 8,
		GetModFiles = 9,
		GetFiles = 10,
		GetModFileDownloadUrl = 11,
	}
}
