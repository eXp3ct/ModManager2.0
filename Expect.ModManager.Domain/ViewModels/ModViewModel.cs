using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expect.ModManager.Domain.ViewModels
{
	public class ModViewModel : IViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string Summary { get; set; }
		public string Author { get; set; }

		public string DateCreated { get; set; }

		public string DateModified { get; set; }
		public bool Selected { get; set; }

		public virtual Mod FullMod { get; set; }
	}
}
