using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Template.Data;

namespace Template.Bot.Modules
{
    public class DevelopmentModule : ModuleBase<SocketCommandContext>
    {
        private readonly TemplateContext _templateContext;

        public DevelopmentModule(TemplateContext templateContext)
        {
            _templateContext = templateContext;
        }

        [Command("connection")]
        public async Task Ping()
        {
            if (await _templateContext.Database.CanConnectAsync())
            {
                await ReplyAsync("Connected to database.");
            }
            else
            {
                await ReplyAsync("Could not connect to database.");
            }
        }

        [Command("assemblies")]
        public async Task Assemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("Template"));
            await ReplyAsync(string.Join(',', assemblies.Select(x => x.FullName)));
        }
    }
}