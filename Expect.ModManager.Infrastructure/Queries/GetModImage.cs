using Expect.ModManager.Domain.Models;
using Expect.ModManager.Net.Common;
using Expect.ModManager.Net.Common.Clients;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Infrastructure.Queries
{
	public class GetModImageQuery : IRequest<Stream>
	{
		public Mod Mod { get; set; }

		public GetModImageQuery(Mod mod)
		{
			Mod = mod;
		}
	}

	public class GetModImageQueryHandler : IRequestHandler<GetModImageQuery, Stream>
	{
		private HttpClient<CurseClient> _client;

		public GetModImageQueryHandler(HttpClient<CurseClient> client)
		{
			_client = client;
		}

		public async Task<Stream> Handle(GetModImageQuery request, CancellationToken cancellationToken)
		{
			var url = request.Mod.Logo.ThumbnailUrl;

			var response = await _client.Client.GetAsync(url, cancellationToken);

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadAsStreamAsync(cancellationToken);
			}

			return null;
		}
	}
}
