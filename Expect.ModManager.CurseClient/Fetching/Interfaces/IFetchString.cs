using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Interfaces;
using Expect.ModManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.CurseApiClient.Fetching.Interfaces
{
	public interface IFetchString
	{
		public Task<string> GetList(IEnumerable<int> modelIds);
	}

	public interface IFetchModString : IFetchString
	{
		public Task<string> SearchMods(
			int gameId, int classId = default,
			int categoryId = default, string gameVersion = "",
			string searchFilter = "", SearchSortFields sortField = SearchSortFields.LastUpdated,
			string sortOrder = "asc", ModLoaderType modLoaderType = default,
			int gameVersionTypeId = default, int authorId = default, string slug = "",
			int index = default, int pageSize = default);
		public Task<string> GetMod(int modId);
	}

	public interface IFetchModFileString : IFetchString
	{
		public Task<string> GetDownloadUrl(int modId, int fileId);
		public Task<string> GetModFile(int modId, int fileId);
		public Task<string> GetModFiles(int modId,
			string gameVersion = null,
			ModLoaderType modLoaderType = ModLoaderType.Any, int index = 0, int pageSize = 0);
	}

	public interface IFetchFeaturesString
	{
		public Task<string> GetCategories(int gameId, int classId = default);
		public Task<string> GetMinecraftModLoaders(string version = default, bool includeAll = default);
		public Task<string> GetMinecraftVersions(bool sortDescending = default);
	}
}
