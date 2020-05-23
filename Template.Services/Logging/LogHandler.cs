using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using Serilog.Events;

namespace Template.Services.Logging
{
    public class LogHandler
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly CommandService _commandService;
        private readonly ILogger _logger;
        
        public LogHandler(
            DiscordSocketClient discordSocketClient, 
            CommandService commandService,
            ILogger logger)
        {
            _discordSocketClient = discordSocketClient;
            _commandService = commandService;
            _logger = logger;

            _discordSocketClient.Log += OnLog;
            _commandService.Log += OnLog;
        }
        
        public Task OnLog(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    _logger.Fatal(message.Exception, message.Message);
                    break;
                
                case LogSeverity.Debug:
                    _logger.Debug(message.ToString());
                    break;
                
                case LogSeverity.Warning:
                    _logger.Warning(message.ToString());
                    break;
                
                case LogSeverity.Error:
                    _logger.Error(message.Exception, message.Message);
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