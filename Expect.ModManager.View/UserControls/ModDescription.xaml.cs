using Expect.ModManager.Domain.Models;
using Expect.ModManager.Infrastructure.Events;
using Expect.ModManager.Infrastructure.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Expect.ModManager.View.UserControls
{
	/// <summary>
	/// Логика взаимодействия для ModDescription.xaml
	/// </summary>
	public partial class ModDescription : UserControl
	{
		public Mod? Mod
		{
			get { return (Mod?)GetValue(ModProperty); }
			set { SetValue(ModProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Mod.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ModProperty =
			DependencyProperty.Register("Mod", typeof(Mod), typeof(ModDescription), new PropertyMetadata(null, OnModChanged));

		public event EventHandler<ModEventArgs> AdditionalInfoRequired;


		public ModDescription()
		{
			InitializeComponent();

			DataContext = this;
		}
		private static void OnModChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ModDescription modDescription && e.NewValue is Mod mod)
			{
				if (mod != null)
				{
					modDescription.OnAdditionalInfoRequired(mod);
				}
			}
		}

		private void OnAdditionalInfoRequired(Mod mod)
		{
			AdditionalInfoRequired?.Invoke(this, new ModEventArgs(mod));
		}

		

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			var url = e.Uri.AbsoluteUri;
			try
			{
				Process.Start(new ProcessStartInfo("chrome.exe", url));
			}
			catch (Win32Exception)
			{
				Process.Start(new ProcessStartInfo(@"C:\Program Files\Google\Chrome\Application\chrome.exe", url));
			}
			e.Handled = true;
		}
	}
}
