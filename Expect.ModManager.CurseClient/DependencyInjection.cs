using Expect.ModManager.CurseClient.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.CurseClient
{
	public static class DependencyInjection
	{
		public static void AddCurseClient(this IServiceCollection services)
		{
			services.AddSingleton<Client>();
		}
	}
}
