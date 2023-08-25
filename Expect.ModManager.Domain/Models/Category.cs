using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Domain.Models
{
	public class Category
	{
		public int Id { get; set; }
		public int GameId { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public string Url { get; set; }
		public string IconUrl { get; set; }
		public DateTime DateModified { get; set; }
		public bool IsClass { get; set; }
		public int? ClassId { get; set; }
		public int? ParentCategoryId { get; set; }
		public int? DisplayIndex { get; set; }
	}
}
