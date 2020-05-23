using Microsoft.Extensions.DependencyInjection;

namespace Template.Services.Logging
{
    public static class LogSetup
    {
        public static IServiceCollection ConfigureLogging(this IServiceCollection serviceCollection)
            => serviceCollection.AddSingleton<LogHandler>();
    }
}