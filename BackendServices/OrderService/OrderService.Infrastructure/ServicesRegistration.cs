using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.RepositoryContract;
using OrderService.Application.Services.Contract;
using OrderService.Application.Services.Implementation;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Persistence.Repositories;

namespace OrderService.Infrastructure
{
    public class ServicesRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            // Register DbContext
            string conStr = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlServer(conStr));

            // Register Repositories
            services.AddScoped<IOrderRepository, OrderRepository>();
            // Register Application Services
            services.AddScoped<IOrderAppService, OrderAppService>();
            // Register AutoMapper
            services.AddAutoMapper(typeof(Application.Mapper.OrderMapper));
        }
    }
}
