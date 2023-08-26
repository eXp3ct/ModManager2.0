using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Domain.Models
{
	public class MinecraftModLoaderIndex : IFeature
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string GameVersion { get; set; }
		public bool Latest { get; set; }
        public bool Recommended { get; set; }
		public DateTime DateModified { get; set; }
		public ModLoaderType Type { get; set; }
    }

	public class MinecraftModLoaderIndexData
	{
		public IEnumerable<MinecraftModLoaderIndex> Data { get; set; }
	}
}
