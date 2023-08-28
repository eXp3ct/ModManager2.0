using Expect.ModManager.Domain.Models;
using Expect.ModManager.Net.Common;
using Expect.ModManager.Net.Common.Clients;
using MediatR;
using Microsoft.Extensions.Logging;
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
		private readonly ILogger<GetModImageQueryHandler> _logger;

		public GetModImageQueryHandler(HttpClient<CurseClient> client, ILogger<GetModImageQueryHandler> logger)
		{
			_client = client;
			_logger = logger;
		}

		public async Task<Stream> Handle(GetModImageQuery request, CancellationToken cancellationToken)
		{
			var url = request.Mod.Logo?.ThumbnailUrl;

			if(url == null)
			{
				return null;
			}

			var response = await _client.Client.GetAsync(url, cancellationToken);

			if (response.IsSuccessStatusCode)
			{
				_logger.LogInformation($"Gained thumbnail image for {request.Mod.Name}");
				return await response.Content.ReadAsStreamAsync(cancellationToken);
			}

			_logger.LogWarning($"Cannot gain thumbnail image for {request.Mod.Name}");

			return null;
		}
	}
}
