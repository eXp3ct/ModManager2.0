using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Test.Fixtures;
using Expect.ModManager.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Expect.ModManager.Test
{
	public class UnitFetchingModFilesTests : IClassFixture<ServiceProviderFixture>
	{
		private readonly IFetchModFileString _fetcher;
		private readonly ViewState _state;

		public UnitFetchingModFilesTests(ServiceProviderFixture fixture)
		{
			_fetcher = fixture.GetServiceProvider().GetRequiredService<IFetchModFileString>();
			_state = fixture.GetServiceProvider().GetRequiredService<ViewState>();
		}

		[Theory]
		[MemberData(nameof(TestIds.ModFileIdList), MemberType = typeof(TestIds))]
		public async Task Fetch_ModFile_NotNull_Or_Empty(int modId, IEnumerable<int> fileIds)
		{
			foreach (var fileId in fileIds)
			{
				var str = await _fetcher.GetModFile(modId, fileId);

				str.Should().NotBeNullOrEmpty();
			}
		}

		[Theory]
		[MemberData(nameof(TestIds.FileIdList), MemberType = typeof(TestIds))]
		public async Task Fetch_ModFiles_List_NotNull_Or_Empty(IEnumerable<int> fileIds)
		{
			var str = await _fetcher.GetList(fileIds);

			str.Should().NotBeNullOrEmpty();
		}

		[Theory]
		[MemberData(nameof(TestIds.ModFileIdList), MemberType = typeof(TestIds))]
		public async Task Fetch_ModFile_DownloadUrl_NotNull_Or_Empty(int modId, IEnumerable<int> fileIds)
		{
			foreach (var fileId in fileIds)
			{
				var str = await _fetcher.GetDownloadUrl(modId, fileId);

				str.Should().NotBeNullOrEmpty();
			}
		}

		[Theory]
		[MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
		public async Task Fetch_ModFiles_NotNull_Or_Empty(IEnumerable<int> modIds)
		{
			foreach (var modId in modIds)
			{
				var str = await _fetcher.GetModFiles(modId, _state.GameVersion, _state.ModLoaderType, 0, pageSize: 50);

				str.Should().NotBeNullOrEmpty();
			}
		}
	}
}
