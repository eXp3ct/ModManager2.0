using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.CurseApiClient.Extensions
{
	public static class HttpResponseMessageExtensions
	{
		public static async Task<string> TryReturnString(this HttpResponseMessage message)
		{
			if (message.IsSuccessStatusCode)
			{
				return await message.Content.ReadAsStringAsync();
			}
			else
			{
				return string.Empty;
			}
		}
	}
}
