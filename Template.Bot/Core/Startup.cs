using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Template.Data;
using Template.Services.Command;
using Template.Services.Logging;
using Template.Services.Startup;

namespace Template
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private ILogger Logger { get; }

        public Startup()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("botSettings.json", true,  false);

            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .WriteTo.Console(theme: SystemConsoleTheme.Literate);

            Configuration = configBuilder.Build();
            Logger = loggerConfig.CreateLogger();
        }

        public static async Task RunAsync(string[] args) => await new Startup().RunAsync();

        private async Task RunAsync()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            serviceProvider.GetRequiredService<CommandHandler>();
            serviceProvider.GetRequiredService<LogHandler>();

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            
            await serviceProvider.GetRequiredService<IHostedService>().StartAsync(cancellationToken);
            await serviceProvider.GetRequiredService<CommandService>().AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);
            
            await Task.Delay(-1, cancellationToken);
        }
        
        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    MessageCacheSize = 1000,
                    AlwaysDownloadUsers = true
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    DefaultRunMode = RunMode.Sync,
                    CaseSensitiveCommands = false
                }))
                .AddHostedService<StartupService>()
                .ConfigureTemplateContext(Configuration["Postgre:ConnectionString"])
                .ConfigureCommand()
                .ConfigureLogging()
                .AddSingleton(Configuration)
                .AddSingleton(Logger);

            foreach (var service in serviceCollection)
            {
                Logger.Information("Registered Service {Service}", service.ServiceType.FullName);
            }
        }
    }
}