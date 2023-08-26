using Expect.ModManager.Domain.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.View.Extensions
{
    public static class ViewModelsExtensions
    {
		public static void AddPlaceholder<IFeatureViewModel>(this IList<IFeatureViewModel> items) where IFeatureViewModel : new()
		{
			items.Insert(0, new IFeatureViewModel());
		}
	}
}
