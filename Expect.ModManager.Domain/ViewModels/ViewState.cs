using Expect.ModManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Domain.ViewModels
{
	public class ViewState
	{
		public int GameId { get; set; }
		public int ClassId { get; set; }
		public int CategoryId { get; set; }
		public string GameVersion { get; set; }
		public string SearchFilter { get; set; }
		public SearchSortFields SortField { get; set; }
		public string SortOrder { get; set; }
		public ModLoaderType ModLoaderType { get; set; }
		public int GameVersionTypeId { get; set; }
		public int AuthorId { get; set; }
		public string Slug { get; set; }
		public int Index { get; set; }
		public int PageSize { get; set; }
	}
}
