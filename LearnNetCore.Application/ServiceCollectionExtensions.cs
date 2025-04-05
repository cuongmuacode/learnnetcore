using LearnNetCore.Application.Articles;
using LearnNetCore.Application.Cache;
using LearnNetCore.Application.Indenties;
using LearnNetCore.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearnNetCore.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLearnNetCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLearnNetCoreDbServices(configuration);
        services.AddSingleton<ICacheService, MemoryCacheService>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}
