using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Template.Services.Command
{
    public class CommandHandler
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
            IConfiguration configuration, ILogger logger)
        {
            _discordSocketClient = discordSocketClient;
            _commandService = commandService;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;

            _discordSocketClient.MessageReceived += HandleCommandAsync;
            _commandService.CommandExecuted += HandleExecutedAsync;
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
                _logger.Warning("Command Exception: {User} tried to run {Command} but: {Error}", 
                    context.User.Username ?? "An unknown user", 
                    commandInfo.Value.Name ?? "an unknown command", 
                    result.ErrorReason ?? "An unknown reason");
            }

            return Task.CompletedTask;
        }
    }
}