using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;

namespace Template.Services
{
    public class LogHandler
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly CommandService _commandService;
        private readonly ILogger _logger;
        
        public LogHandler(DiscordSocketClient discordSocketClient, CommandService commandService, ILogger logger)
        {
            _discordSocketClient = discordSocketClient;
            _commandService = commandService;
            _logger = logger;

            _discordSocketClient.Log += OnLogAsync;
            _commandService.Log += OnLogAsync;
        }
        
        public Task OnLogAsync(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    _logger.Fatal(message.Exception, message.Message ?? "An exception bubbled up: ");
                    break;
                
                case LogSeverity.Debug:
                    _logger.Debug(message.ToString());
                    break;
                
                case LogSeverity.Warning:
                    _logger.Warning(message.ToString());
                    break;
                
                case LogSeverity.Error:
                    _logger.Error(message.Exception, message.Message ?? "An exception bubbled up: ");
                    break;
                
                case LogSeverity.Info:
                    _logger.Information(message.ToString());
                    break;
                
                case LogSeverity.Verbose:
                    _logger.Verbose(message.ToString());
                    break;
            }

            return Task.CompletedTask;
        }
    }
}