using Expect.ModManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Infrastructure.Events
{
	public class ModEventArgs : EventArgs
	{
		public Mod Mod { get; set; }

		public ModEventArgs(Mod mod)
		{
			Mod = mod;
		}
	}
}
