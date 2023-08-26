using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Domain.Interfaces
{
	public interface IMod : IModel
	{
		public string Name { get; set; }
	}
}
