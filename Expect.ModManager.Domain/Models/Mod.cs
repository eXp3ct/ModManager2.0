using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Domain.Models
{
	public class Mod : IMod
	{
		public int Id { get; set; }
		public int GameId { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public ModLinks Links { get; set; }
		public string Summary { get; set; }
		public ModStatus Status { get; set; }
		public long DownloadCount { get; set; }
		public bool IsFeatured { get; set; }
		public int PrimaryCategoryId { get; set; }
		public List<Category> Categories { get; set; }
		public int? ClassId { get; set; }
		public List<ModAuthor> Authors { get; set; }
		public ModAsset Logo { get; set; }
		public List<ModAsset> ScreenShots { get; set; }
		public int MainFileId { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		public DateTime DateReleased { get; set; }
		public int GamePopularityRank { get; set; }
		public bool IsAvailable { get; set; }
		public int ThumsbUpCount { get; set; }
	}

	public class ModData
	{
		public Mod Data { get; set; }
	}

	public class ModsData
	{
		public IEnumerable<Mod> Data { get; set; }
	}
}
