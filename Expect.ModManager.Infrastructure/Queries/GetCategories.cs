using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expect.ModManager.CurseApiClient.Deserialization.Interfaces;
using Expect.ModManager.Domain.Interfaces;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Domain.ViewModels.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Infrastructure.Queries
{
	public class GetFeatureQuery<TFeatureViewModel> : IRequest<IList<TFeatureViewModel>>
		where TFeatureViewModel : IFeatureViewModel
	{
		public ViewState ViewState { get; set; }

		public GetFeatureQuery(ViewState viewState)
		{
			ViewState = viewState;
		}
	}

	public class GetFeatureQueryHandler<TFeature, TFeatureViewModel> : IRequestHandler<GetFeatureQuery<TFeatureViewModel>, IList<TFeatureViewModel>>
		where TFeatureViewModel : IFeatureViewModel
		where TFeature : IFeature
	{
		private readonly IFeaturesDeserizlier _featuresDeserizlier;
		private readonly IMapper _mapper;
		private readonly ILogger<GetFeatureQueryHandler<TFeature, TFeatureViewModel>> _logger;

		public GetFeatureQueryHandler(IFeaturesDeserizlier featuresDeserizlier, IMapper mapper, ILogger<GetFeatureQueryHandler<TFeature, TFeatureViewModel>> logger)
		{
			_featuresDeserizlier = featuresDeserizlier;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<IList<TFeatureViewModel>> Handle(GetFeatureQuery<TFeatureViewModel> request, CancellationToken cancellationToken)
		{
			var features = await _featuresDeserizlier.GetFeature<TFeature>(request.ViewState);

			_logger.LogInformation($"Gained {typeof(TFeature).Name}");

			return features
				.AsQueryable()
				.ProjectTo<TFeatureViewModel>(_mapper.ConfigurationProvider)
				.ToList();
		}
	}
}
