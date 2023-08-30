using Expect.ModManager.Caching.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Domain.ViewModels.Interfaces;
using Expect.ModManager.Infrastructure.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Expect.ModManager.Caching.Providers
{
	public class ModProvider : IModProvider
	{
		private readonly IMemoryCache _memoryCache;
		private readonly ViewState _viewState;
		private readonly IMediator _mediator;
		private readonly ILogger<ModProvider> _logger;
		private readonly IList<Mod> _selectedMods;

		public ModProvider(
			IMemoryCache memoryCache,
			ViewState viewState,
			IMediator mediator,
			ILogger<ModProvider> logger,
			IList<Mod> selectedMods)
		{
			_memoryCache = memoryCache;
			_viewState = viewState;
			_mediator = mediator;
			_logger = logger;
			_selectedMods = selectedMods;
		}

		public async Task<IEnumerable<IViewModel>> GetMods()
		{
			var mods = await _memoryCache.GetOrCreateAsync(_viewState.ToString(), async entry =>
			{
				_logger.LogWarning($"Cant retrive mods from cache, fetching...\n Key: {_viewState}");

				var query = new SearchModsQuery(_viewState);

				return await _mediator.Send(query);
			});

			if (mods == null)
				return Enumerable.Empty<IViewModel>();

			var list = new List<IViewModel>();

			foreach (var mod in mods)
			{
				if (_selectedMods.Select(m => m.Id).Contains(mod.Id))
				{
					((ModViewModel)mod).Selected = true;
				}
				list.Add(mod);
			}

			return list;
		}

	}
}
