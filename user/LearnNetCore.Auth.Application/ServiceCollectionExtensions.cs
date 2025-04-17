using LearnNetCore.Auth.Application.Identities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearnNetCore.Auth.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLearnNetCoreServices(this IServiceCollection services, IConfiguration configuration)
    {  
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}
