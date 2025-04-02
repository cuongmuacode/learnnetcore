using LearnNetCore.Application;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace LearnNetCore.Api;
/// <summary>
/// 
/// </summary>
public class Program
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "LearnNetCore API",
                Description = "API for LearnNetCore",
            });
        });

        builder.Services.AddControllers();
        var configuration = builder.Configuration;
        builder.Services.AddLearnNetCoreServices(configuration);

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger(options =>
            {
                options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
            });
            app.UseSwaggerUI(options =>
            // UseSwaggerUI is called only in Development.
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.MapControllers();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.Run();
    }
}
