using Expect.ModManager.CurseApiClient.Fetching.Interfaces;
using Expect.ModManager.Domain.Models;
using MediatR;

namespace Expect.ModManager.Infrastructure.Queries
{
	public class SearchModsQuery : IRequest<IEnumerable<Mod>>
	{
	}

	public class SearchModsQueryHandler : IRequestHandler<SearchModsQuery, IEnumerable<Mod>>
	{
		private readonly IGetFeaturesResponse _featuresResponse;

		public SearchModsQueryHandler(IGetFeaturesResponse featuresResponse)
		{
			_featuresResponse = featuresResponse;
		}

		public async Task<IEnumerable<Mod>> Handle(SearchModsQuery request, CancellationToken cancellationToken)
		{
			var features = await _featuresResponse.GetMinecraftVersions(true);

			features.Any();

			return new List<Mod>();
		}
	}
}
