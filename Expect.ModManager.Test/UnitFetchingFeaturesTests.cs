using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.Test.Fixtures;

using Microsoft.Extensions.DependencyInjection;

namespace Expect.ModManager.Test
{
	public class UnitFetchingFeaturesTests : IClassFixture<ServiceProviderFixture>
	{
		private readonly IFetchFeaturesString _fetcher;

		public UnitFetchingFeaturesTests(ServiceProviderFixture fixture)
		{
			_fetcher = fixture.GetServiceProvider().GetRequiredService<IFetchFeaturesString>();
		}

		[Fact]
		public async Task Fetch_Categories_NotNull_Or_Empty()
		{
			var gameId = 432;
			var classId = 6;

			var str = await _fetcher.GetCategories(gameId, classId);
			
			str.Should().NotBeNullOrEmpty();
		}

		[Fact]
		public async Task Fetch_Minecraft_Game_Verions_NotNull_Or_Empty()
		{
			var str = await _fetcher.GetMinecraftVersions();

			str.Should().NotBeNullOrEmpty();
		}

		[Fact]
		public async Task Fetch_Minecraft_Mod_Loaders_NotNull_Or_Empty()
		{
			var gameVersion = "1.12.2";

			var str = await _fetcher.GetMinecraftModLoaders(gameVersion);

			str.Should().NotBeNullOrEmpty();
		}
	}
}