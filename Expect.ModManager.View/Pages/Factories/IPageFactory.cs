using Expect.ModManager.View.Pages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Expect.ModManager.View.Pages.Factories
{
	public interface IPageFactory<TPage> where TPage : Page
	{
		public Task<TPage> Create();
	}
}
