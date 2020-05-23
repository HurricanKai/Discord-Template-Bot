using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Template.Services.Startup
{
    public class StartupService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public StartupService(DiscordSocketClient discordSocketClient, IConfiguration configuration, ILogger logger)
        {
            _discordSocketClient = discordSocketClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            var token = _configuration["Discord:Token"];

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.Fatal("The bot Token was not found at the specified location.");
                return;
            }

            try
            {
                await _discordSocketClient.LoginAsync(TokenType.Bot, token);
                await _discordSocketClient.StartAsync();
            }
            catch (Exception e)
            {
                _logger.Fatal(e.Message);
            }
        }
    }
}