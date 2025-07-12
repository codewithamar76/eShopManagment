using Catalog.Application.RepoContract;
using Catalog.Application.ServiceContract;
using Catalog.Application.Services;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure
{
    public class ServicesRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration _config)
        {
            // Register the DbContext
            services.AddDbContext<CategoryDBContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));
            // Register repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            
            // Add other necessary services here
            services.AddScoped<IProductServices, ProductServices>();

            // Register AutoMapper
            services.AddAutoMapper(typeof(Application.Helper.ProductMapper));
        }
    }
}
