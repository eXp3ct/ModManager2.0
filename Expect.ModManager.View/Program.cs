using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Expect.ModManager.View
{
	public class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

			var app = host.Services.GetRequiredService<App>();
			app?.Run();
		}

		private static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog((host, config) =>
				{
					config
						.WriteTo.File($"logs/logs.log", rollingInterval: RollingInterval.Hour)
						.MinimumLevel.Debug();
				})
				.ConfigureAppConfiguration((context, configBuilder) =>
				{
					configBuilder
						.SetBasePath(context.HostingEnvironment.ContentRootPath);
				})
				.ConfigureServices((context, services) =>
				{
					services.AddSingleton<App>();
					services.AddSingleton<MainWindow>();
					services.ConfigureServices(context.Configuration);
				});
	}
}
