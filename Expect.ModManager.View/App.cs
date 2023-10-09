using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace Expect.ModManager.View
{
	public partial class App : Application
	{
		private readonly MainWindow _mainWindow;
		private readonly ILogger<App> _logger;

		public App(MainWindow mainWindow, ILogger<App> logger)
		{
			_mainWindow = mainWindow;
			_logger = logger;
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			_mainWindow.Show();
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			base.OnStartup(e);
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.ExceptionObject.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			_logger.LogCritical((Exception)e.ExceptionObject, "Unhandled eception occured");
		}

		protected override void OnExit(ExitEventArgs e)
		{
			_mainWindow.Close();
			AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
			base.OnExit(e);
		}
	}
}
