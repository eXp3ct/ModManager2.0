using Expect.ModManager.CurseApiClient.Deserialization.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Test.Fixtures;
using Expect.ModManager.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace Expect.ModManager.Test
{
	public class UnitDeserializationModFileTests : IClassFixture<ServiceProviderFixture>
	{
		private readonly IModFileDeserializer _deserializer;
		private readonly ViewState _state;

		public UnitDeserializationModFileTests(ServiceProviderFixture fixture)
		{
			_deserializer = fixture.GetServiceProvider().GetRequiredService<IModFileDeserializer>();
			_state = fixture.GetServiceProvider().GetRequiredService<ViewState>();
		}

		[Theory]
		[MemberData(nameof(TestIds.FileIdList), MemberType = typeof(TestIds))]
		public async Task Deserialize_Files_NotNull_And_NotEmpty(IEnumerable<int> fileIds)
		{
			var files = await _deserializer.GetList(fileIds);

			files.Should().NotBeNull().And.AllBeOfType<ModFile>();
		}

		[Theory]
		[MemberData(nameof(TestIds.ModFileIdList), MemberType = typeof(TestIds))]
		public async Task Deserialize_File_DownloadUrl_NotNull_And_NotEmpty(int modId, IEnumerable<int> fileIds)
		{
			foreach (var fileId in fileIds)
			{
				var url = await _deserializer.GetDownloadUrl(modId, fileId);

				url.Should().NotBeNull();
			}
		}

		[Theory]
		[MemberData(nameof(TestIds.ModFileIdList), MemberType = typeof(TestIds))]
		public async Task Deserilize_ModFile_NotNull(int modId, IEnumerable<int> fileIds)
		{
			foreach(var fileId in fileIds)
			{
				var file = await _deserializer.GetModFile(modId, fileId);

				file.Should().NotBeNull().And.BeOfType<ModFile>();
			}
		}

		[Theory]
		[MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
		public async Task Deserialize_Mod_Files_NotNull_And_Not_Empty(IEnumerable<int> modIds)
		{
			foreach(var modId in modIds)
			{
				var files = await _deserializer.GetModFiles(modId, _state, 0, 50);

				files.Should().NotBeNull().And.AllBeOfType<ModFile>().And.HaveCountLessThanOrEqualTo(50);	
			}
		}
	}
}
