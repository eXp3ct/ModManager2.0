using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Infrastructure.Queries;
using Expect.ModManager.View.Pages;
using Expect.ModManager.View.Pages.Factories;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Expect.ModManager.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly IPageFactory<DataPage> _pageFactory;
		private readonly ViewState _viewState;

		public MainWindow(IPageFactory<DataPage> pageFactory, ViewState viewState)
		{
			InitializeComponent();
			_pageFactory = pageFactory;
			_viewState = viewState;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			MainFrame.Content = _pageFactory.Create();
		}
	}
}
