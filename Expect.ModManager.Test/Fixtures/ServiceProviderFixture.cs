
using Expect.ModManager.View;

namespace Expect.ModManager.Test.Fixtures
{
	public class ServiceProviderFixture
	{

		public IServiceProvider GetServiceProvider()
		{
			return Program.CreateHostBuilder(Array.Empty<string>()).Build().Services;
		}
	}
}
