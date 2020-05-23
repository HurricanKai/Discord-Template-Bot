using Microsoft.Extensions.DependencyInjection;

namespace Template.Services.Command
{
    public static class CommandSetup
    {
        public static IServiceCollection ConfigureCommand(this IServiceCollection serviceCollection)
            => serviceCollection.AddSingleton<CommandHandler>();
    }
}