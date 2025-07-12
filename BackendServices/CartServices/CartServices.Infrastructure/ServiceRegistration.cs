using CartServices.Application.RepositoryInterface;
using CartServices.Application.Services.Implementation;
using CartServices.Application.Services.Interface;
using CartServices.Infrastructure.Persistence;
using CartServices.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartServices.Infrastructure
{
    public class ServiceRegistration
    {
        public static void RegisteredServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CartDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            // Repositories
            services.AddScoped<ICartRepository, CartRepository>();

            // Application Services
             services.AddScoped<ICartService, CartService>(); // Uncomment if you have a CartService implementation

            // AutoMapper
            services.AddAutoMapper(typeof(Application.Mapper.CartMapper).Assembly); // Ensure you have a CartMapper in your Application.Mapper namespace

            
        }
    }
}
