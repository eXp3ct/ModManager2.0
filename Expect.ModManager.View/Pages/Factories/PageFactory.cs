using Expect.ModManager.View.Pages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Expect.ModManager.View.Pages.Factories
{
	public class PageFactory<TPage> : IPageFactory<TPage> where TPage : Page, IFillable
	{
		private readonly Func<TPage> _factory;

		public PageFactory(Func<TPage> factory)
		{
			_factory = factory;
		}

		public async Task<TPage> Create()
		{
			var page = _factory();

			await page.Fill();

			return page;
		}
	}
}
