using System.ComponentModel;

namespace Expect.ModManager.Domain.ViewModels.Interfaces
{
	public interface IViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

	}
}
