using Expect.ModManager.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expect.ModManager.CurseClient.Common;
using System.Collections.ObjectModel;

namespace Expect.ModManager.Infrastructure.Queries
{
	public class SearchModsQuery : IRequest<IEnumerable<Mod>>
	{
	}

	public class SearchModsQueryHandler : IRequestHandler<SearchModsQuery, IEnumerable<Mod>>
	{
		private readonly Client _client;

		public SearchModsQueryHandler(Client client)
		{
			_client = client;
		}

		public async Task<IEnumerable<Mod>> Handle(SearchModsQuery request, CancellationToken cancellationToken)
		{
			var response = await _client.GetAsync($"https://api.curseforge.com/v1/mods/222880");

			var str = response.Content.ReadAsStringAsync();

			return new ObservableCollection<Mod>();
		}
	}
}
