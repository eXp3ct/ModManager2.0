using Expect.ModManager.Domain.Enums;

namespace Expect.ModManager.CurseApiClient.Fetching.Interfaces
{
	public interface IGetModsResponse
	{
		public Task<string> GetMod(int modId);
		public Task<string> GetModDescription(int modId);
		public Task<string> GetMods(IEnumerable<int> modIds);
		public Task<string> SearchMods(
			int gameId, int classId = default,
			int categoryId = default, string gameVersion = "",
			string searchFilter = "", SearchSortFields sortField = SearchSortFields.LastUpdated,
			string sortOrder = "asc", ModLoaderType modLoaderType = default,
			int gameVersionTypeId = default, int authorId = default, string slug = "",
			int index = default, int pageSize = default);
	}
}
