using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Template.Services.Command
{
    public class CommandHandler : IHostedService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        
        public CommandHandler(
            DiscordSocketClient discordSocketClient, 
            CommandService commandService, 
            IServiceProvider serviceProvider, 
            IConfiguration configuration, ILogger<CommandHandler> logger)
        {
            _discordSocketClient = discordSocketClient;
            _commandService = commandService;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage socketUserMessage) || socketUserMessage.Author.IsBot)
            {
                return;
            }
            
            var context = new SocketCommandContext(_discordSocketClient, socketUserMessage);
            await TryExecute(socketUserMessage, context);
        }
        
        private async Task TryExecute(IUserMessage message, SocketCommandContext context)
        {
            var prefix = _configuration["Discord:Prefix"];
            var argPos = 0; 
                
            if (message.HasStringPrefix(prefix, ref argPos))
            {
                await _commandService.ExecuteAsync(context, argPos, _serviceProvider);
            }
        }
        
        private Task HandleExecutedAsync(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                const string backupCommandStr = "an unknown command";
                var commandStr = commandInfo.IsSpecified ? commandInfo.Value?.Name ?? backupCommandStr : backupCommandStr;
                _logger.LogWarning("Command Exception: {User} tried to run {Command} but: {Error}", 
                    context.User?.Username ?? "An unknown user", 
                    commandStr, 
                    result.ErrorReason ?? "An unknown reason");
            }

            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _discordSocketClient.MessageReceived += HandleCommandAsync;
            _commandService.CommandExecuted += HandleExecutedAsync;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _discordSocketClient.MessageReceived -= HandleCommandAsync;
            _commandService.CommandExecuted -= HandleExecutedAsync;
            
            return Task.CompletedTask;
        }
    }
}