using Expect.ModManager.CurseApiClient.Deserialization.Interfaces;
using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Expect.ModManager.CurseApiClient.Deserialization
{
	public class ModDeserializer : IModDeserializer
	{
		private readonly IFetchModString _modString;

		public ModDeserializer(IFetchModString modString)
		{
			_modString = modString;
		}

		public async Task<Mod> GetMod(int modelId)
		{
			var json = await _modString.GetMod(modelId);

			var data = JsonConvert.DeserializeObject<ModData>(json);

			return data!.Data;
		}

		public async Task<IEnumerable<Mod>> GetModelsList(IEnumerable<int> modelIds)
		{
			var json = await _modString.GetList(modelIds);

			var data = JsonConvert.DeserializeObject<ModsData>(json);

			return data!.Data;
		}

		public async Task<IEnumerable<Mod>> SearchMods(int gameId, int classId = 0,
			int categoryId = 0, string gameVersion = null,
			string searchFilter = null, SearchSortFields sortField = 0,
			string sortOrder = "asc", ModLoaderType modLoaderType = ModLoaderType.Any,
			int gameVersionTypeId = 0, int authorId = 0, string slug = null,
			int index = 0, int pageSize = 0)
		{
			var json = await _modString.SearchMods(gameId, classId,
				categoryId, gameVersion, searchFilter,
				sortField, sortOrder, modLoaderType, gameVersionTypeId, authorId, slug, index, pageSize);

			var data = JsonConvert.DeserializeObject<ModsData> (json);

			return data!.Data;
		}
	}

	public class ModFileDeserializer : IModFileDeserializer
	{
		private IFetchModFileString _modFileString;

		public ModFileDeserializer(IFetchModFileString modFileString)
		{
			_modFileString = modFileString;
		}

		public async Task<string> GetDownloadUrl(int modId, int fileId)
		{
			return await _modFileString.GetDownloadUrl(modId, fileId);
		}

		public async Task<ModFile> GetModFile(int modelId, int fileId)
		{
			var json = await _modFileString.GetModFile(modelId, fileId);

			var data = JsonConvert.DeserializeObject<ModFileData>(json);

			return data!.Data;
		}

		public async Task<IEnumerable<ModFile>> GetModelsList(IEnumerable<int> modelIds)
		{
			var json = await _modFileString.GetList(modelIds);

			var data = JsonConvert.DeserializeObject<ModFilesData>(json);

			return data!.Data;
		}

		public async Task<IEnumerable<ModFile>> GetModFiles(int modId, string gameVersion = null, ModLoaderType modLoaderType = ModLoaderType.Any, int index = 0, int pageSize = 0)
		{
			var json = await _modFileString.GetModFiles(modId, gameVersion, modLoaderType, index, pageSize);

			var data = JsonConvert.DeserializeObject<ModFilesData>(json);

			return data!.Data;
		}
	}

	public class FeaturesDeserializer : IFeaturesDeserizlier
	{
		private readonly IFetchFeaturesString _featuresString;

		public FeaturesDeserializer(IFetchFeaturesString featuresString)
		{
			_featuresString = featuresString;
		}

		public async Task<IEnumerable<Category>> GetCategories(int gameId, int classId = 0)
		{
			var json = await _featuresString.GetCategories(gameId, classId);

			var data = JsonConvert.DeserializeObject<CategoryData>(json);

			return data!.Data;
		}

		public async Task<IEnumerable<MinecraftGameVersion>> GetMinecraftGameVersions(bool sortDescending = false)
		{
			var json = await _featuresString.GetMinecraftVersions(sortDescending);

			var data = JsonConvert.DeserializeObject<MinecraftGameVersionData>(json);

			return data!.Data;
		}

		public async Task<IEnumerable<MinecraftModLoaderIndex>> GetMinecraftModLoaders(string version = null, bool includeAll = true)
		{
			var json = await _featuresString.GetMinecraftModLoaders(version, includeAll);

			var data = JsonConvert.DeserializeObject<MinecraftModLoaderIndexData>(json);

			return data!.Data;
		}
	}
}
