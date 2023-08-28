using Expect.ModManager.Net.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Net.Downloading
{
	public interface IDownloader<TClient> where TClient : HttpClient, IClient
	{
		public Task<byte[]> Download(string url);
	}
}
