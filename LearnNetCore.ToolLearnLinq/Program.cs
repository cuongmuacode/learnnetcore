using LearnNetCore.Application;
using LearnNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearnNetCore.ToolLearnLinq
{
    public class Program
    {
        private static ServiceProvider _serviceProvider { get; set; }
        private static IConfigurationRoot _configuration;

        static async Task Main(string[] args)
        {
            SetupServices();
            IServiceScope scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var stock = dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == Guid.Parse("6768EF62-AED6-43F8-B4BF-08DD71BE2FEA"));

            var stocks = dbContext.Stocks.Where(x => x.CompanyName.Contains("S"));
            var countStocks = dbContext.Stocks.Count();

            var stocksPages = dbContext.Stocks.OrderBy(x => x.Id);


            DisposeServices();
        }

        private static void SetupServices()
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(_configuration);
            services.AddLearnNetCoreServices(_configuration);
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
