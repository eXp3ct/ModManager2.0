using Expect.ModManager.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Expect.ModManager.View
{
	public partial class App : Application
	{
		private readonly MainWindow _mainWindow;
		private readonly ILogger<App> _logger;
        private readonly IList<Mod> _selectedMods;
        

        public App(MainWindow mainWindow, ILogger<App> logger, IList<Mod> selectedMods)
        {
            _mainWindow = mainWindow;
            _logger = logger;
            _selectedMods = selectedMods;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _mainWindow.Show();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (Directory.Exists("backup"))
            {
                if (File.Exists("backup/backup.json"))
                {
                    var result = MessageBox.Show("Предыдущяя сессия была заверешена некорректно, желаете загрузить последние выделенные моды?", "Восстановление сессии", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        _selectedMods.Clear();
                        var json = File.ReadAllText("backup/backup.json");

                        var mods = JsonConvert.DeserializeObject<List<Mod>>(json);

                        mods.ForEach(x => _selectedMods.Add(x));

                    }

                    File.Delete("backup/backup.json");
                }
            }

            base.OnStartup(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var json = JsonConvert.SerializeObject(_selectedMods, Formatting.Indented);

            if (!Directory.Exists("backup"))
            {
                Directory.CreateDirectory("backup");
            }

            File.WriteAllText("backup/backup.json", json);

            MessageBox.Show(e.ExceptionObject.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            _logger.LogCritical((Exception)e.ExceptionObject, "Unhandled eception occured");

            Current.Shutdown();
        }
        protected override void OnExit(ExitEventArgs e)
		{
			_mainWindow.Close();
			AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
			base.OnExit(e);
		}
	}
}
