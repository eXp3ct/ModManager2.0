﻿using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Infrastructure.Events;
using Expect.ModManager.Infrastructure.Queries;
using Expect.ModManager.View.Pages.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Expect.ModManager.View.Pages
{
	/// <summary>
	/// Логика взаимодействия для DataPage.xaml
	/// </summary>
	public partial class DataPage : Page, IFillable
	{
		private readonly IMediator _mediator;
		private readonly ViewState _viewState;

		public DataPage(IMediator mediator, ViewState viewState)
		{
			InitializeComponent();
			_mediator = mediator;
			_viewState = viewState;

			_viewState.PropertyChanged += OnViewStatePropertyChanged;
		}

		private void OnViewStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			Fill();
		}

		public async void Fill()
		{
			var query = new SearchModsQuery(_viewState);

			var mods = await _mediator.Send(query);

			DataGrid.ItemsSource = mods;
		}

		private async void ModDescription_DepenencyRequired(object sender, ModEventArgs e)
		{
			ModDescription.DependencyList.ItemsSource = await GetDependenicesNames(e.Mod);
			ModDescription.ModImage.Source = await GetModImage(e.Mod);
		}

		private async Task<ImageSource> GetModImage(Mod mod)
		{
			var query = new GetModImageQuery(mod);

			var stream = await _mediator.Send(query);

			if(stream == null)
				return new BitmapImage();

			var bitmap = new BitmapImage();
			bitmap.BeginInit();
			bitmap.CacheOption = BitmapCacheOption.OnLoad;
			bitmap.StreamSource = stream;
			bitmap.EndInit();
			bitmap.Freeze();

			return bitmap;
		}

		private async Task<IEnumerable<string>> GetDependenicesNames(Mod mod)
		{
			var query = new GetModDependenciesQuery(mod);

			var mods = await _mediator.Send(query);

			var names = mods.Select(x => x.Name);

			return names;
		}


	}
}
