using Expect.ModManager.CurseApiClient.Urls.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.CurseApiClient.Urls
{
	public interface IGetEndpoint
	{
		public Uri GetEndpoint(RequestType requestType, int? modId = null, int? fileId = null);
	}
}
