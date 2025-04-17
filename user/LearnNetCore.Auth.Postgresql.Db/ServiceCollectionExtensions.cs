using LearnNetCore.Auth.Application.Identities;
using LearnNetCore.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearnNetCore.Auth.Postgresql.Db;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLearnNetCoreAuthPostgresqlDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("NpgsqlConnection");
        Console.WriteLine(connectionString);

        services.AddIdentity<ApplicationUser, IdentityRole>(
              options =>
              {
                  options.Password.RequireDigit = true;
                  options.Password.RequireLowercase = true;
                  options.Password.RequireUppercase = true;
                  options.Password.RequireNonAlphanumeric = true;
                  options.Password.RequiredLength = 6;
              }
          ).AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("LearnNetCore.Postgresql.Db"));
        });
        
        return services;
    }
}
