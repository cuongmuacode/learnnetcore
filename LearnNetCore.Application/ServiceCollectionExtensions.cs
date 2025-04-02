using LearnNetCore.Application.Implements;
using LearnNetCore.Application.Interfaces;
using LearnNetCore.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearnNetCore.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLearnNetCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLearnNetCoreDbServices(configuration);
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        return services;
    }
}
