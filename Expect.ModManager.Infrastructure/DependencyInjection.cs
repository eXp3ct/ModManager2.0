using Expect.ModManager.Domain.Interfaces;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using Expect.ModManager.Domain.ViewModels.Interfaces;
using Expect.ModManager.Infrastructure.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Expect.ModManager.Infrastructure
{
	public static class DependencyInjection
	{
		public static void AddInfrastructure(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddGenericHanlder<Category, CategoryViewModel>();
			services.AddGenericHanlder<MinecraftGameVersion, MinecraftGameVersionViewModel>();
		}

		private static void AddGenericHanlder<TFeature, TFeatureViewModel>(this IServiceCollection services)
			where TFeature : IFeature
			where TFeatureViewModel : IFeatureViewModel
		{
			services.AddTransient<
				IRequestHandler<GetFeatureQuery<TFeatureViewModel>, IList<TFeatureViewModel>>,
				GetFeatureQueryHandler<TFeature, TFeatureViewModel>
				>();
		}
	}
}
