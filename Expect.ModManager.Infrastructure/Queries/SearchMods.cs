using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expect.ModManager.CurseApiClient.Deserialization.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Domain.ViewModels.Interfaces;
using MediatR;
using System.Collections.ObjectModel;

namespace Expect.ModManager.Infrastructure.Queries
{
	public class SearchModsQuery : IRequest<IEnumerable<IViewModel>>
	{
		public ViewState ViewState { get; set; }

		public SearchModsQuery(ViewState viewState)
		{
			ViewState = viewState;
		}
	}

	public class SearchModsQueryHandler : IRequestHandler<SearchModsQuery, IEnumerable<IViewModel>>
	{
		private readonly IModDeserializer _modDeserializer;
		private readonly IMapper _mapper;

		public SearchModsQueryHandler(IModDeserializer modDeserializer, IMapper mapper)
		{
			_modDeserializer = modDeserializer;
			_mapper = mapper;
		}

		public async Task<IEnumerable<IViewModel>> Handle(SearchModsQuery request, CancellationToken cancellationToken)
		{
			var mods = await _modDeserializer
				.SearchMods(request.ViewState);

			return mods
				.AsQueryable()
				.ProjectTo<ModViewModel>(_mapper.ConfigurationProvider);
		}
	}
}
