using Expect.ModManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Domain.Models
{
	public class ModFile
	{
		public int Id { get; set; }
		public int GameId { get; set; }
		public int ModId { get; set; }
		public bool IsAvaliable { get; set; }
		public string DisplayName { get; set; }
		public string FileName { get; set; }
		public ModFileReleaseType ReleaseType { get; set; }
		public ModFileStatus FileStatus { get; set; }
		public DateTime FileDate { get; set; }
		public long FileLength { get; set; }
		public long DonwloadCount { get; set; }
		public long? FileSizeOnDisk { get; set; }
		public string DownloadUrl { get; set; }
		public List<string> GameVersions { get; set; }
		public List<ModFileDependency> Dependencies { get; set; }
	}
}
