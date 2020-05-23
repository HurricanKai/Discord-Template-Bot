using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Template.Data;
using Template.Services.Command;
using Template.Services.Logging;
using Template.Services.Startup;

namespace Template
{
    internal class Program
    {
        private static Task Main(string[] args) => Host.CreateDefaultBuilder(args)
            .UseSerilog((context, configuration) =>
            {
                configuration
                    .Enrich.FromLogContext()
                    .MinimumLevel.Verbose()
                    .WriteTo.Console(theme: SystemConsoleTheme.Literate);
            })
            .ConfigureServices(((context, collection) =>
            {
                var configuration = context.Configuration;
                collection.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Verbose,
                        MessageCacheSize = 1000,
                        AlwaysDownloadUsers = true
                    }))
                    .AddSingleton(provider =>
                    {
                        var commandService = new CommandService(new CommandServiceConfig
                        {
                            LogLevel = LogSeverity.Verbose,
                            DefaultRunMode = RunMode.Sync,
                            CaseSensitiveCommands = false
                        });

                        commandService.AddModulesAsync(Assembly.GetEntryAssembly(), provider);

                        return commandService;
                    })
                    .ConfigureTemplateContext(configuration["Postgre:ConnectionString"])
                    .ConfigureStartup()
                    .ConfigureCommand()
                    .ConfigureLogging();
            }))
            .RunConsoleAsync();
    }
}