using Expect.ModManager.Caching;
using Expect.ModManager.CurseApiClient;
using Expect.ModManager.Domain.Configurations;
using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Infrastructure;
using Expect.ModManager.Net;
using Expect.ModManager.Updates;
using Expect.ModManager.View.Pages;
using Expect.ModManager.View.Pages.Factories;
using Expect.ModManager.View.Pages.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace Expect.ModManager.View
{
	public static class Startup
	{
		public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<CurseClientOptions>(configuration.GetSection(nameof(CurseClientOptions)));
			services.Configure<AutoUpdaterOptions>(configuration.GetSection(nameof(AutoUpdaterOptions)));

			services.AddInfrastructure();
			services.AddCurseClient();
			services.AddHttpClients();

			services.AddFillablePageFactory<DataPage>();

			services.AddSingleton(new ViewState
			{
				GameId = 432,
				ClassId = 6,
				Index = 0,
				PageSize = 10,
				SortField = SearchSortFields.TotalDownloads,
				SortOrder = "desc",
			});

			services.AddSingleton<IList<Mod>, ObservableCollection<Mod>>();
			services.AddSingleton(x =>
			{
				Directory.CreateDirectory("settings");
				if (!File.Exists("settings/favorites.json")){
					using var _ = File.Create("settings/favorites.json");
					//_.Dispose();
				}
				var collection = new ObservableCollection<Mod>();
				var json = File.ReadAllText(configuration["UserSettingsLocation"]);
				var mods = JsonConvert.DeserializeObject<IEnumerable<Mod>>(json);

				if (mods == null)
					return collection;

				foreach(var mod in mods)
					collection.Add(mod);

				return collection;
			});

			services.AddInMemoryCaching();
			services.AddAutoUpdates();
		}

		private static void AddPageFactory<TPage>(this IServiceCollection services) where TPage : Page
		{
			services.AddSingleton<TPage>();
			services.AddSingleton<Func<TPage>>(x => () => x.GetRequiredService<TPage>());
			services.AddSingleton<IPageFactory<TPage>, PageFactory<TPage>>();
		}

		private static void AddFillablePageFactory<TPage>(this IServiceCollection services) where TPage : Page, IFillable
		{
			services.AddSingleton<TPage>();
			services.AddSingleton<Func<TPage>>(x => () => x.GetRequiredService<TPage>());
			services.AddSingleton<IFIllablePageFactory<TPage>, FillablePageFactory<TPage>>();
		}
	}
}
