using AutoMapper;
using AutoMapper.QueryableExtensions;
using Expect.ModManager.Caching.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Domain.ViewModels.Interfaces;
using Expect.ModManager.Infrastructure.Events;
using Expect.ModManager.Infrastructure.Queries;
using Expect.ModManager.View.Pages.Interfaces;
using Expect.ModManager.View.UserControls;
using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
		private readonly IList<Mod> _selectedModIds;
		private readonly IModProvider _modProvider;
		private readonly IMapper _mapper;

 		public event EventHandler<ReportEventArgs> Report;
		public static event EventHandler DoneInstalling;

		public DataPage(IMediator mediator, ViewState viewState, IList<Mod> selectedModIds, IModProvider modProvider, IMapper mapper)
		{
			InitializeComponent();
			_mediator = mediator;
			_viewState = viewState;

			_viewState.PropertyChanged += OnViewStatePropertyChanged;
			_selectedModIds = selectedModIds;
			_modProvider = modProvider;
			_mapper = mapper;
		}

		private async void OnViewStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			await Fill();
		}

		public async Task Fill()
		{
			var mods = await _modProvider.GetMods();

			var collection = new ObservableCollection<IViewModel>();
			foreach(var mod in mods)
			{
				collection.Add(mod);
			}
			collection.CollectionChanged += Collection_CollectionChanged;
			DataGrid.ItemsSource = collection;
		}

		public void Fill(IEnumerable<Mod> mods)
		{
			var list = mods
						.AsQueryable()
						.ProjectTo<ModViewModel>(_mapper.ConfigurationProvider);
			var result = new List<ModViewModel>();

			foreach(var mod in list)
			{
				mod.Selected = true;
				result.Add(mod);
			}

			DataGrid.ItemsSource = result;
		}

		private void Collection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			MessageBox.Show("Something Changed");
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

			if (stream == null)
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

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			var mod = (ModViewModel)DataGrid.SelectedItem;

			if (mod == null)
				return;

			_selectedModIds.Add(mod.FullMod);
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			var mod = (ModViewModel)DataGrid.SelectedItem;

			if (mod == null || !_selectedModIds.Contains(mod.FullMod))
				return;

			_selectedModIds.Remove(mod.FullMod);
		}

		private async void ModDescription_StartInstallingMods(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(_viewState.FolderPath))
			{
				MessageBox.Show("Выберите путь до папки(Файл -> Выбрать папку)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				OnDoneInstalling();
				return;
			}

			var query = new InstallModsQuery();
			query.Report += ProgressReport;
			var errors = await _mediator.Send(query);
			if (errors.Any())
			{
				MessageBox.Show($"{_selectedModIds.Count} модов и их зависимости успешно установлены в\n{_viewState.FolderPath}," +
					$"но несколько модов не удалось установить попробуйте установить их вручную:" +
					$"\n{string.Join('\n', errors
					.Select(pair => $"• Мод: {pair.Key.Name} | Файл: {pair.Value.DisplayName} | Ссылка: {pair.Key.Links.WebSiteUrl}"))}"
					, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				OnDoneInstalling();
				return;
			}

			MessageBox.Show($"{_selectedModIds.Count} модов и их зависимости успешно установлены в\n{_viewState.FolderPath}",
				"Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

			OnDoneInstalling();
		}

		private void ProgressReport(object? sender, ReportEventArgs e)
		{
			Report?.Invoke(sender, e);
		}

		private void OnDoneInstalling()
		{
			DoneInstalling?.Invoke(this, EventArgs.Empty);
		}
	}
}
