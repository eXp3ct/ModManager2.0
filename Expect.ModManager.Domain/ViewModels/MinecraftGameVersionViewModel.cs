using Expect.ModManager.Domain.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Domain.ViewModels
{
	public class MinecraftGameVersionViewModel : IFeatureViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
