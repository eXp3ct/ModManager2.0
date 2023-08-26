using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Interfaces;

namespace Expect.ModManager.Domain.Models
{
	public class MinecraftGameVersion : IFeature
	{
		public int Id { get; set; }
		public int GameVersionId { get; set; }
		public string VersionString { get; set; }
		public string JarDownloadUrl { get; set; }
		public string JsonDownloadUrl { get; set; }
		public bool Approved { get; set; }
		public DateTime DateModified { get; set; }
		public GameVersionStatus GameVersionStatus { get; set; }
	}

	public class MinecraftGameVersionData
	{
		public IEnumerable<MinecraftGameVersion> Data { get; set; }
	}
}
