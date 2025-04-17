using LearnNetCore.Application.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearnNetCore.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLearnNetCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICacheService, MemoryCacheService>();
        return services;
    }
}
