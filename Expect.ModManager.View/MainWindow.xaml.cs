using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Domain.ViewModels.Interfaces;
using Expect.ModManager.Infrastructure.Queries;
using Expect.ModManager.Updates;
using Expect.ModManager.View.Extensions;
using Expect.ModManager.View.Pages;
using Expect.ModManager.View.Pages.Factories;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Expect.ModManager.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const int PaginationLimit = 10000;

		private readonly IFIllablePageFactory<DataPage> _pageFactory;
		private readonly ViewState _viewState;
		private readonly IMediator _meditaor;
		private readonly IList<Mod> _selectedModIds;
		private readonly Updater _updater;
		private readonly ObservableCollection<Mod> _favoritesMods;

		private DataPage _dataPage;
		private int _currentPage = 1;
		private bool _selectedModsView = false;

		public MainWindow(
			IFIllablePageFactory<DataPage> pageFactory,
			ViewState viewState,
			IMediator meditaor,
			IList<Mod> selectedModIds,
			Updater updater,
			ObservableCollection<Mod> favoritesMods)
		{
			InitializeComponent();
			_pageFactory = pageFactory;
			_viewState = viewState;
			_meditaor = meditaor;
			_selectedModIds = selectedModIds;
			_updater = updater;
			_favoritesMods = favoritesMods;
		}

		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Title += $" v{Assembly.GetExecutingAssembly().GetName().Version}";
			_dataPage = await _pageFactory.Create();

			_dataPage.Report += OnProgressReport;
			DataPage.DoneInstalling += OnDoneInstalling;
			MainFrame.Content = _dataPage;

			await AddFeatures(_viewState);

			SortOrderCheckBox.IsChecked = true;

			if (_selectedModIds is ObservableCollection<Mod> observable)
			{
				observable.CollectionChanged += SelectedModsChanged;
			}

			_updater.CheckForUpdates();

			SortFieldsComboBox.SelectedItem = _viewState.SortField;
			PageSizeComboBox.SelectedItem = _viewState.PageSize;

			
        }

		private void OnDoneInstalling(object? sender, EventArgs e)
		{
			InstallingProgressBar.Value = 0;
			_selectedModIds.Clear();
		}

		private void OnProgressReport(object? sender, Infrastructure.Events.ReportEventArgs e)
		{
			InstallingProgressBar.Value += e.NewValue;
		}

		private void SelectedModsChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			SelectedModsCountText.Text = _selectedModIds.Count.ToString();
		}

		private async Task AddFeatures(ViewState state)
		{
			await AddFeature<CategoryViewModel>(state);
			await AddFeature<MinecraftGameVersionViewModel>(state);
			AddModLoaders();
			AddSortFields();
			AddPageSize();
		}

		private async Task AddFeature<TFeatureViewModel>(ViewState state)
			where TFeatureViewModel : IFeatureViewModel, new()
		{
			var query = new GetFeatureQuery<TFeatureViewModel>(state);

			var features = await _meditaor.Send(query);

			features.AddPlaceholder();
			var comboBox = FindName($"{typeof(TFeatureViewModel).Name}ComboBox") as System.Windows.Controls.ComboBox;

			comboBox!.ItemsSource = features;
			comboBox.DisplayMemberPath = nameof(IFeatureViewModel.Name);
		}

		private void AddModLoaders()
		{
			var types = Enum.GetValues<ModLoaderType>();

			ModLoadersComboBox.ItemsSource = types;
		}

		private void AddSortFields()
		{
			var fields = Enum.GetValues<SearchSortFields>();

			SortFieldsComboBox.ItemsSource = fields;
		}

		private void AddPageSize()
		{
			var sizes = new List<int> { 5, 10, 15, 20, 30, 50 };

			PageSizeComboBox.ItemsSource = sizes;
		}

		private void CategoryChanged(object sender, SelectionChangedEventArgs e)
		{
			_viewState.CategoryId = ((CategoryViewModel)CategoryViewModelComboBox.SelectedItem).Id;
		}

		private void GameVersionChanged(object sender, SelectionChangedEventArgs e)
		{
			_viewState.GameVersion = ((MinecraftGameVersionViewModel)MinecraftGameVersionViewModelComboBox.SelectedItem).Name;
			ModLoadersComboBox.SelectedItem ??= ModLoaderType.Forge;
		}

		private void ModLoaderChanged(object sender, SelectionChangedEventArgs e)
		{
			var type = (ModLoaderType)(ModLoadersComboBox.SelectedItem);
			_viewState.ModLoaderType = type;
		}

		private void SortFieldChanged(object sender, SelectionChangedEventArgs e)
		{
			_viewState.SortField = (SearchSortFields)(SortFieldsComboBox.SelectedItem);
		}

		private void SearchFilterChanged(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key != Key.Enter)
				return;

			_viewState.SearchFilter = SearchFilterTextBox.Text;
		}

		private void PageSizeChanged(object sender, SelectionChangedEventArgs e)
		{
			_viewState.PageSize = (int)PageSizeComboBox.SelectedItem;
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			_viewState.SortOrder = "desc";
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			_viewState.SortOrder = "asc";
		}

		private void PageBack(object sender, RoutedEventArgs e)
		{
			if (_currentPage == 1)
				return;

			CurrentPageText.Text = (--_currentPage).ToString();
			_viewState.Index -= _viewState.PageSize;
		}

		private void PageForward(object sender, RoutedEventArgs e)
		{
			if (_viewState.Index + _viewState.PageSize > PaginationLimit)
				return;

			CurrentPageText.Text = (++_currentPage).ToString();
			_viewState.Index += _viewState.PageSize;
		}

		private void ChooseDirectory(object sender, RoutedEventArgs e)
		{
			using var dialog = new FolderBrowserDialog();

			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				_viewState.FolderPath = dialog.SelectedPath;
			}
		}

		private async void ViewSelectedMods(object sender, RoutedEventArgs e)
		{
			if (_selectedModsView)
			{
				await _dataPage.Fill();
				_selectedModsView = false;
				ViewSelectedModsButton.Content = "Посмотреть";
				return;
			}
			ViewSelectedModsButton.Content = "Назад";
			_dataPage.Fill(_selectedModIds);
			_selectedModsView = true;
		}

		private async void ExportSelectedMods(object sender, RoutedEventArgs e)
		{
			using var dialog = new FolderBrowserDialog();

			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				var path = dialog.SelectedPath;

				var query = new ExportSelectedModsQuery(path);

				var savedPath = await _meditaor.Send(query);

				System.Windows.MessageBox.Show($"Выделенные моды сохранены в\n{savedPath}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private async void ImportSelectedMods(object sender, RoutedEventArgs e)
		{
			using var dialog = new OpenFileDialog()
			{
				Filter = "*|*.json",
			};

			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				var filePath = dialog.FileName;

				var query = new ImportSelectedModsQuery(filePath);

				var importedMods = await _meditaor.Send(query);

				if (importedMods == null)
					return;

				_dataPage.Fill(importedMods);
			}
		}

		private void CheckForUpdatesManually(object sender, RoutedEventArgs e)
		{
			_updater.CheckForUpdates();
		}

		private void ViewFavoritesMods(object sender, RoutedEventArgs e)
		{
			_dataPage.Fill(_favoritesMods);
		}

		private async void ViewSearchMods(object sender, RoutedEventArgs e)
		{
			await _dataPage.Fill();
		}
	}
}
