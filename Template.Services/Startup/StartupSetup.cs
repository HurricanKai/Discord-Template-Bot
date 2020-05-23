using Microsoft.Extensions.DependencyInjection;

namespace Template.Services.Startup
{
    public static class StartupSetup
    {
        public static IServiceCollection ConfigureStartup(this IServiceCollection serviceCollection)
            => serviceCollection.AddHostedService<StartupService>();
    }
}