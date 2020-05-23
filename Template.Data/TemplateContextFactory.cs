using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Template.Data
{
    public class TemplateContextFactory : IDesignTimeDbContextFactory<TemplateContext>
    {
        public TemplateContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<TemplateContext>()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseNpgsql(configuration["PostgreConnectionString"]);
            
            return new TemplateContext(optionsBuilder.Options);
        }
    }
}