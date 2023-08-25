using Expect.ModManager.Domain.Interfaces;

namespace Expect.ModManager.Domain.Models
{
	public class Category : IModel
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

	public class CategoryData
	{
		public IEnumerable<Category> Data { get; set;}
	}
}
