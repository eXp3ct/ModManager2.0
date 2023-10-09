using Expect.ModManager.CurseApiClient.Urls;
using Expect.ModManager.CurseApiClient.Urls.Enums;
using Expect.ModManager.Test.Fixtures;
using Expect.ModManager.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Test
{
	public class UnitCurseEndpointsTests : IClassFixture<ServiceProviderFixture>
	{
		private readonly IGetEndpoint _endpoint;

        public UnitCurseEndpointsTests(ServiceProviderFixture fixture)
        {
            _endpoint = fixture.GetServiceProvider().GetRequiredService<IGetEndpoint>();
        }

        [Theory]
        [MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
        public void Get_Mod_Endpoint_Success(IEnumerable<int> modIds)
        {
            var requestType = RequestType.GetMod;

            foreach(var  modId in modIds)
            {
                var expectedUri = new Uri($"https://api.curseforge.com/v1/mods/{modId}");
                var endpoint = _endpoint.GetEndpoint(requestType, modId);

				endpoint.Should().NotBeNull().And.BeOfType<Uri>().And.Be(expectedUri);
			}
        }

        [Fact]
        public void Get_Search_Mods_Endpoint_Success()
        {
            var requestType = RequestType.SearchMod;
            var expectedUri = new Uri("https://api.curseforge.com/v1/mods/search");


            var endpoint = _endpoint.GetEndpoint(requestType);

			endpoint.Should().NotBeNull().And.BeOfType<Uri>().And.Be(expectedUri);
		}

        [Fact]
        public void Get_Mods_Endpoint_Success()
        {
            var requestType = RequestType.GetMods;
            var expectedUri = new Uri("https://api.curseforge.com/v1/mods");

            var endpoint = _endpoint.GetEndpoint(requestType);

			endpoint.Should().NotBeNull().And.BeOfType<Uri>().And.Be(expectedUri);
		}

		[Theory]
		[MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
        public void Get_Mod_Description_Endpoint_Success(IEnumerable<int> modIds)
        {
            var requestType = RequestType.GetModDescription;

            foreach(var modId in modIds)
            {
                var expectedUri = new Uri($"https://api.curseforge.com/v1/mods/{modId}/description");
                var endpoint = _endpoint.GetEndpoint(requestType, modId);

				endpoint.Should().NotBeNull().And.BeOfType<Uri>().And.Be(expectedUri);
			}
        }

        [Fact]
        public void Get_Categories_Endpoint_Success()
        {
            var requestType = RequestType.GetCategories;
            var expectedUri = new Uri("https://api.curseforge.com/v1/categories");

            var endpoint = _endpoint.GetEndpoint(requestType);

            endpoint.Should().NotBeNull().And.BeOfType<Uri>().And.Be(expectedUri);
        }

		[Fact]
		public void Get_Minecraft_Versions_Endpoint_Success()
		{
			var requestType = RequestType.GetMinecraftVersions;
            var expectedUri = new Uri("https://api.curseforge.com/v1/minecraft/version");

			var endpoint = _endpoint.GetEndpoint(requestType);

			endpoint.Should().NotBeNull().And.BeOfType<Uri>().And.Be(expectedUri);
		}

		[Fact]
		public void Get_Minecraft_Mod_Loaders_Endpoint_Success()
		{
			var requestType = RequestType.GetMinecraftModLoaders;
            var expectedUri = new Uri("https://api.curseforge.com/v1/minecraft/modloader");

			var endpoint = _endpoint.GetEndpoint(requestType);

			endpoint.Should().NotBeNull().And.BeOfType<Uri>().And.Be(expectedUri);
		}

        [Theory]
		[MemberData(nameof(TestIds.ModFileIdList), MemberType = typeof(TestIds))]
        public void Get_ModFile_Endpoint_Success(int modId, IEnumerable<int> fileIds)
        {
            var requestType = RequestType.GetModFile;

            foreach(var fileId in fileIds)
            {
                var expectedUri = new Uri($"https://api.curseforge.com/v1/mods/{modId}/files/{fileId}");

                var endpoint = _endpoint.GetEndpoint(requestType, modId, fileId);

                endpoint.Should().NotBeNull().And.BeOfType<Uri>().And.Be(expectedUri);
            }
        }

		[Theory]
		[MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
        public void Get_Mod_Files_Endpoint_Success(IEnumerable<int> modIds)
        {
            var requestType = RequestType.GetModFiles;

            foreach(var modId in modIds)
            {
                var expectedUri = new Uri($"https://api.curseforge.com/v1/mods/{modId}/files");

                var endpoint = _endpoint.GetEndpoint(requestType, modId);

                endpoint.Should().NotBeNull().And.BeOfType<Uri>().And.Be(expectedUri);
            }
        }

        [Fact]
        public void Get_Files_Endpoint_Success()
        {
            var requestType = RequestType.GetFiles;
            var expectedUri = new Uri("https://api.curseforge.com/v1/mods/files");

            var endpoint = _endpoint.GetEndpoint(requestType);

            endpoint.Should().NotBeNull().And.BeOfType<Uri>().And.Be(expectedUri);
        }

		[Theory]
		[MemberData(nameof(TestIds.ModFileIdList), MemberType = typeof(TestIds))]
        public void Get_ModFile_DownloadUrl_Endpoint_Success(int modId, IEnumerable<int> fileIds)
        {
            var requestType = RequestType.GetModFileDownloadUrl;

            foreach(var fileId in fileIds)
            {
                var expectedUri = new Uri($"https://api.curseforge.com/v1/mods/{modId}/files/{fileId}/download-url");

                var endpoint = _endpoint.GetEndpoint(requestType, modId, fileId);

				endpoint.Should().NotBeNull().And.BeOfType<Uri>().And.Be(expectedUri);
			}
        }
	}
}
