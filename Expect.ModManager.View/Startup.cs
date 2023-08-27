using Expect.ModManager.CurseApiClient;
using Expect.ModManager.Domain.Configurations;
using Expect.ModManager.Domain.Enums;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Infrastructure;
using Expect.ModManager.Net;
using Expect.ModManager.View.Pages;
using Expect.ModManager.View.Pages.Factories;
using Expect.ModManager.View.Pages.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Expect.ModManager.View
{
	public static class Startup
	{
		public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<CurseClientOptions>(configuration.GetSection(nameof(CurseClientOptions)));

			services.AddInfrastructure();
			services.AddCurseClient();
			services.AddHttpClients();

			services.AddPageFactory<DataPage>();

			services.AddSingleton(new ViewState
			{
				GameId = 432,
				ClassId = 6,
				ModLoaderType = ModLoaderType.Forge,
				GameVersion = "1.12.2",
				Index = 0,
				PageSize = 10,
				SortOrder = "desc",
			});

			services.AddSingleton<IList<int>, ObservableCollection<int>>();
		}

		private static void AddPageFactory<TPage>(this IServiceCollection services) where TPage : Page, IFillable
		{
			services.AddSingleton<TPage>();
			services.AddSingleton<Func<TPage>>(x => () => x.GetRequiredService<TPage>());
			services.AddSingleton<IPageFactory<TPage>, PageFactory<TPage>>();
		}
	}
}
