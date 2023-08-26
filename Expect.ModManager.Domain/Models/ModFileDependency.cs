using Expect.ModManager.Domain.Enums;

namespace Expect.ModManager.Domain.Models
{
	public class ModFileDependency
	{
		public int ModId { get; set; }
		public ModFileReleationType RelationType { get; set; }
	}
}
