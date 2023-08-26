using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Infrastructure.Queries;
using Expect.ModManager.View.Pages.Interfaces;
using MediatR;
using System.ComponentModel;
using System.Windows.Controls;

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
	}
}
