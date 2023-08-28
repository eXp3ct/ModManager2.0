using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Infrastructure.Events
{
	public class ReportEventArgs : EventArgs
	{
		public double NewValue { get; set; }

		public ReportEventArgs(double newValue)
		{
			NewValue = newValue;
		}
	}
}
