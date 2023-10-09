using Expect.ModManager.CurseApiClient.Deserialization.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Test.Fixtures;
using Expect.ModManager.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.ModManager.Test
{
	public class UnitDeserializationModTests : IClassFixture<ServiceProviderFixture>
	{
		private readonly IModDeserializer _deserializer;
		private readonly ViewState _state;

		public UnitDeserializationModTests(ServiceProviderFixture fixture)
		{
			_deserializer = fixture.GetServiceProvider().GetRequiredService<IModDeserializer>();
			_state = fixture.GetServiceProvider().GetRequiredService<ViewState>();
		}

		[Theory]
		[MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
		public async Task Deserialize_Mod_List_NotNull_And_NotEmpty(IEnumerable<int> modIds)
		{
			var mods = await _deserializer.GetList(modIds);

			mods.Should().NotBeNullOrEmpty().And.AllBeOfType<Mod>();
		}

		[Theory]
		[MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
		public async Task Deserialize_Mod_NotNull(IEnumerable<int> modIds)
		{
			foreach (var modId in modIds)
			{
				var mod = await _deserializer.GetMod(modId);

				mod.Should().NotBeNull();
			}
		}

		[Fact]
		public async Task Deserialize_Search_Mods_NotNull_And_NotEmpty()
		{
			var mods = await _deserializer.SearchMods(_state);

			mods.Should().NotBeNullOrEmpty().And.AllBeOfType<Mod>();
		}
	}
}
