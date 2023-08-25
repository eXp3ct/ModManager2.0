using Expect.ModManager.Domain.ViewModels.Interfaces;

namespace Expect.ModManager.Domain.ViewModels
{
	public class ModViewModel : IViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public Uri Link { get; set; }

		public string Summary { get; set; }
		public string Author { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime DateModified { get; set; }
	}
}
