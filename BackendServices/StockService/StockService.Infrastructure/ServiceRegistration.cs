using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockService.Application.RepositoryContract;
using StockService.Application.Services.Contract;
using StockService.Application.Services.Implementation;
using StockService.Infrastructure.Persistence;
using StockService.Infrastructure.Persistence.RepositoryImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Infrastructure
{
    public class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            // Register application services
            services.AddScoped<IStockAppService, StockAppService>();
            // Register repository
            services.AddScoped<IStockRepository, StockRepository>();
            // Register DbContext
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<StockDbContext>(options =>
                options.UseSqlServer(connectionString)
            );
        }
    }
}
