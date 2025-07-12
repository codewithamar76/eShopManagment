using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.RepositoryContract;
using PaymentService.Application.Services.Contract;
using PaymentService.Application.Services.Implementation;
using PaymentService.Infrastructure.Persistance;
using PaymentService.Infrastructure.Persistance.RepositoryImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure
{
    public class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            string ConnString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<PaymentDbContext>(options =>
            {
                options.UseSqlServer(ConnString);
            });

            // Register the PaymentRepository as a singleton
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            // Register the RazorPayService as a singleton
            //services.AddSingleton<IRazorPayService, RazorPayService>();
            // Register the PaymentService as a singleton
            services.AddScoped<IPaymentAppService, PaymentAppService>();
            services.AddAutoMapper(typeof(Application.Mapper.PaymentMapper).Assembly);
        }
    }
}
