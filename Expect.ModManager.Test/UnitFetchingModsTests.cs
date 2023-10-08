using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Test.Fixtures;
using Expect.ModManager.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.ModManager.Test
{
	public class UnitFetchingModsTests : IClassFixture<ServiceProviderFixture>
	{
		private readonly IFetchModString _fetcher;
		private readonly ViewState _state;

		public UnitFetchingModsTests(ServiceProviderFixture fixture)
		{
			_fetcher = fixture.GetServiceProvider().GetRequiredService<IFetchModString>();
			_state = fixture.GetServiceProvider().GetRequiredService<ViewState>();
		}

		[Theory]
		[InlineData(69163)] // Thermal Expansion
		public async Task Fetch_Mod_NotNull_Or_Empty(int modId)
		{
			var str = await _fetcher.GetMod(modId);

			str.Should().NotBeNullOrEmpty();
		}

		[Theory]
		[MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
		public async Task Fetch_Mods_NotNull_Or_Empty(IEnumerable<int> modIds)
		{
			var str = await _fetcher.GetList(modIds);

			str.Should().NotBeNullOrEmpty();
		}

		[Fact]
		public async Task Fetch_Search_Mods_NotNull_Or_Empty()
		{
			var str = await _fetcher.SearchMods(
				_state.GameId, _state.ClassId, _state.CategoryId, _state.GameVersion,
				_state.SearchFilter, _state.SortField, _state.SortOrder, _state.ModLoaderType,
				_state.GameVersionTypeId, _state.AuthorId, _state.Slug, _state.Index, _state.PageSize);

			str.Should().NotBeNullOrEmpty();
		}
	}
}
