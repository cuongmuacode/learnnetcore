using LearnNetCore.Application;
using LearnNetCore.Application.Articles;
using LearnNetCore.EntityFrameworkCore.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearnNetCore.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLearnNetCoreDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlConnection");
        Console.WriteLine(connectionString);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddLearnNetCoreServices(configuration);
        return services;
    }
}
