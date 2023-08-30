using AutoUpdaterDotNET;
using Expect.ModManager.Domain.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Reflection;

namespace Expect.ModManager.Updates
{
	public class Updater : IAuthentication
	{
		private readonly IOptions<AutoUpdaterOptions> _options;

		public Updater(IOptions<AutoUpdaterOptions> options)
		{
			_options = options;			

			AutoUpdater.InstallationPath = Assembly.GetExecutingAssembly().Location;
			AutoUpdater.LetUserSelectRemindLater = false;
			AutoUpdater.ClearAppDirectory = true;
		}

		public void CheckForUpdates()
		{
			if (EnsureAuth())
			{
				AutoUpdater.Start(_options.Value.UpdateFileUrl);
				
			}
		}

		public bool EnsureAuth()
		{
			AutoUpdater.BasicAuthXML ??= this;
			AutoUpdater.BasicAuthDownload ??= this;

			return true;
		}

		public void Apply(ref MyWebClient webClient)
		{
		
			webClient.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {_options.Value.ApiKey}");
		}

	}
}
