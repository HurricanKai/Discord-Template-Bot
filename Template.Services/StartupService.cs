using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Template.Services
{
    public class StartupService
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly IConfiguration _configuration;

        public StartupService(DiscordSocketClient discordSocketClient, IConfiguration configuration)
        {
            _discordSocketClient = discordSocketClient;
            _configuration = configuration;
        }

        public async Task StartAsync()
        {
            var token = _configuration["Discord:Token"];

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("Bot Token Not Found");
            }

            await _discordSocketClient.LoginAsync(TokenType.Bot, token);
            await _discordSocketClient.StartAsync();
        }
    }
}