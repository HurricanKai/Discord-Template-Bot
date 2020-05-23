using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Template.Services.Logging
{
    public class LogHandler : IHostedService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly CommandService _commandService;
        private readonly ILogger _logger;
        
        public LogHandler(DiscordSocketClient discordSocketClient, CommandService commandService, ILogger<LogHandler> logger)
        {
            _discordSocketClient = discordSocketClient;
            _commandService = commandService;
            _logger = logger;
        }
        
        public Task OnLogAsync(LogMessage message)
        {
            var level = message.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Trace,
                LogSeverity.Debug => LogLevel.Debug,
                _ => throw new ArgumentOutOfRangeException(nameof(message.Severity))
            };
            
            _logger.Log(level, message.Exception, message.Message);

            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _discordSocketClient.Log += OnLogAsync;
            _commandService.Log += OnLogAsync;
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _discordSocketClient.Log -= OnLogAsync;
            _commandService.Log -= OnLogAsync;
            
            return Task.CompletedTask;
        }
    }
}