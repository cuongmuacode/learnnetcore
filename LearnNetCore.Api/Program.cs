using LearnNetCore.Application;
using LearnNetCore.Application.Exceptions;
using LearnNetCore.Application.Identities;
using LearnNetCore.Application.Models;
using LearnNetCore.Domain;
using LearnNetCore.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text.Json;

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
        var configuration = builder.Configuration;
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "LearnNetCore API",
                Description = "API for LearnNetCore",
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }

                }
            );
        });

        builder.Services.AddControllers();
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
            options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
            }
        ).AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
            options.DefaultScheme =
            options.DefaultSignInScheme =
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(
            options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
                    )
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {

                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(
                            new Response
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = context.Error,
                            }
                        );

                        await context.Response.WriteAsync(result);

                    }
                };
            }
         );

        var cors = builder.Configuration.GetSection("Cors").Get<string[]>();
        Console.WriteLine(cors);
        builder.Services.AddCors(options =>
            options.AddPolicy(
                "AngularApplication",
                policy =>
                policy.WithOrigins(cors)
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyHeader()
            )
        );

        builder.Services.AddAuthorization(
            options => options.FallbackPolicy =
                new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()
        );
        builder.Services.AddLearnNetCoreDbServices(configuration);


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

        app.UseCors("AngularApplication");
        app.MapControllers();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        app.Run();
    }
}
