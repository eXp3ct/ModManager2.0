using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Domain.Configurations
{
	public class AutoUpdaterOptions
	{
		public string ApiKey { get; set; }
		public string UpdateFileUrl { get; set; }
	}
}
