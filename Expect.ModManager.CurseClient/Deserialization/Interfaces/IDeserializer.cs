using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;

namespace Expect.ModManager.CurseApiClient.Deserialization.Interfaces
{
	public interface IBaseDeserializer<TModel> where TModel : IModel
	{
		public Task<IEnumerable<TModel>> GetModelsList(IEnumerable<int> modelIds);
		
	}
	
	public interface IModFileDeserializer : IBaseDeserializer<ModFile>
	{
		public Task<string> GetDownloadUrl(int modId, int fileId);
		public Task<ModFile> GetModFile(int modId, int fileId);
		public Task<IEnumerable<ModFile>> GetModFiles(int modId,
			string gameVersion = null,
			ModLoaderType modLoaderType = ModLoaderType.Any, int index = 0, int pageSize = 0);

		public Task<IEnumerable<ModFile>> GetModFiles(int modId, ViewState state, int index = 0, int pageSize = 0) =>
			GetModFiles(modId, state.GameVersion, state.ModLoaderType, index, pageSize);
	}

	public interface IModDeserializer : IBaseDeserializer<Mod>
	{
		public Task<IEnumerable<Mod>> SearchMods(int gameId, int classId = default,
			int categoryId = default, string gameVersion = default, string searchFilter = default,
			SearchSortFields sortField = default, string sortOrder = "asc", ModLoaderType modLoaderType = default,
			int gameVersionTypeId = default, int authorId = default, string slug = default, int index = default,
			int pageSize = default);
		public Task<IEnumerable<Mod>> SearchMods(ViewState viewState) =>
			SearchMods(viewState.GameId, viewState.ClassId, viewState.CategoryId, viewState.GameVersion,
				viewState.SearchFilter, viewState.SortField, viewState.SortOrder, viewState.ModLoaderType,
				viewState.GameVersionTypeId, viewState.AuthorId, viewState.Slug, viewState.Index, viewState.PageSize);

		public Task<Mod> GetMod(int modId);
	}

	public interface IFeaturesDeserizlier
	{
		public Task<IEnumerable<MinecraftGameVersion>> GetMinecraftGameVersions(bool sortDescending = false);
		public Task<IEnumerable<MinecraftModLoaderIndex>> GetMinecraftModLoaders(string version = null, bool includeAll = true);
		public Task<IEnumerable<Category>> GetCategories(int gameId, int classId = 0);

		public async Task<IEnumerable<TFeature>> GetFeature<TFeature>(ViewState state) where TFeature : IFeature => typeof(TFeature) switch
		{
			Type t when t == typeof(Category) => (IEnumerable<TFeature>)await GetCategories(state.GameId, state.ClassId),
			Type t when t == typeof(MinecraftGameVersion) => (IEnumerable<TFeature>)await GetMinecraftGameVersions(state.SortOrder != "asc"),
			Type t when t == typeof(MinecraftModLoaderIndex) => (IEnumerable<TFeature>)await GetMinecraftModLoaders(state.GameVersion, false),
			_ => new List<TFeature>(),
		};
	}
}
