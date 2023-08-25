using Expect.ModManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.CurseApiClient.Fetching.Interfaces
{
	public interface IGetModFilesResponse
	{
		public Task<string> GetFiles(IEnumerable<int> fileIds);
		public Task<string> GetModFile(int modId, int fileId);
		public Task<string> GetModFileDownloadUrl(int modId, int fileId);
		public Task<string> GetModFiles(int modId, string gameVersion = default,
			ModLoaderType modLoaderType = ModLoaderType.Any, int index = default, int pageSize = default);
	}
}
