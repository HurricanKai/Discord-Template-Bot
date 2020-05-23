using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Template.Services.Startup
{
    public class StartupService : IHostedService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public StartupService(DiscordSocketClient discordSocketClient, IConfiguration configuration, ILogger<StartupService> logger)
        {
            _discordSocketClient = discordSocketClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var token = _configuration["Discord:Token"];

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogCritical("The bot Token was not found at the specified location.");
                return;
            }

            try
            {
                await _discordSocketClient.LoginAsync(TokenType.Bot, token);
                await _discordSocketClient.StartAsync();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _discordSocketClient.LogoutAsync();
            await _discordSocketClient.StopAsync();
        }
    }
}