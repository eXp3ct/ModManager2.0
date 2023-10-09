using Expect.ModManager.CurseApiClient.Deserialization.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Test.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.ModManager.Test
{
	public class UnitDeserializtionFeatureTests : IClassFixture<ServiceProviderFixture>
	{
		private readonly IFeaturesDeserizlier _deserizlier;
		private readonly ViewState _state;

		public UnitDeserializtionFeatureTests(ServiceProviderFixture fixture)
		{
			_deserizlier = fixture.GetServiceProvider().GetRequiredService<IFeaturesDeserizlier>();
			_state = fixture.GetServiceProvider().GetRequiredService<ViewState>();
		}

		[Fact]
		public async Task Deserialize_Categories_NotNull_And_NotEmpty()
		{
			var gameId = 432;
			var classId = 6;

			var categories = await _deserizlier.GetCategories(gameId, classId);

			categories.Should().NotBeNullOrEmpty().And.AllBeOfType<Category>();
		}

		[Fact]
		public async Task Deserialize_Minecraft_Versions_NotNull_And_NotEmpty()
		{
			var version = await _deserizlier.GetMinecraftGameVersions();

			version.Should().NotBeNullOrEmpty().And.AllBeOfType<MinecraftGameVersion>();
		}

		[Fact]
		public async Task Deserialize_Minecraft_ModLoaders_NotNull_And_NotEmpty()
		{
			var gameVersion = "1.12.2";

			var modloads = await _deserizlier.GetMinecraftModLoaders(gameVersion);

			modloads.Should().NotBeNullOrEmpty().And.AllBeOfType<MinecraftModLoaderIndex>();
		}

		[Fact]
		public async Task Deserialize_Feature_Category()
		{
			var categories = await _deserizlier.GetFeature<Category>(_state);

			categories.Should().NotBeNullOrEmpty().And.AllBeOfType<Category>();
		}

		[Fact]
		public async Task Deserialize_Feature_Minecraft_GameVersions()
		{
			var versions = await _deserizlier.GetFeature<MinecraftGameVersion>(_state);

			versions.Should().NotBeNullOrEmpty().And.AllBeOfType<MinecraftGameVersion>();
		}

		[Fact]
		public async Task Deserialize_Feature_Minecraft_ModLoaders()
		{
			var loaders = await _deserizlier.GetFeature<MinecraftModLoaderIndex>(_state);

			loaders.Should().NotBeNullOrEmpty().And.AllBeOfType<MinecraftModLoaderIndex>();
		}
	}
}
