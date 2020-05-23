using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Data
{
    public static class TemplateContextSetup
    {
        public static IServiceCollection ConfigureTemplateContext(this IServiceCollection serviceCollection, string connectionString)
            => serviceCollection.AddDbContext<TemplateContext>(options => options.UseNpgsql(connectionString));
    }
}