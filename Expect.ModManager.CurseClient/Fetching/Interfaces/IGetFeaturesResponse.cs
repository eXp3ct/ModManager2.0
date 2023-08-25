using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.CurseApiClient.Fetching.Interfaces
{
	public interface IGetFeaturesResponse
	{
		public Task<string> GetCategories(int gameId, int classId = default);
		public Task<string> GetMinecraftModLoaders(string version = default, bool includeAll = default);
		public Task<string> GetMinecraftVersions(bool sortDescending = default);
	}
}
