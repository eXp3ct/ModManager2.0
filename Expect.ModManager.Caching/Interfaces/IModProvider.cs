using Expect.ModManager.Domain.ViewModels.Interfaces;

namespace Expect.ModManager.Caching.Interfaces
{
	public interface IModProvider
	{
		public Task<IEnumerable<IViewModel>> GetMods();
	}
}
