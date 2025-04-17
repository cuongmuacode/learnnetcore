using LearnNetCore.Application;
using LearnNetCore.Application.Articles;
using LearnNetCore.EntityFrameworkCore;
using LearnNetCore.EntityFrameworkCore.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearnNetCore.Postgresql.Db;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLearnNetCorePostgresqlDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("NpgsqlConnection");
        Console.WriteLine(connectionString);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("LearnNetCore.Postgresql.Db"));
        });

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddLearnNetCoreServices(configuration);
        return services;
    }
}
