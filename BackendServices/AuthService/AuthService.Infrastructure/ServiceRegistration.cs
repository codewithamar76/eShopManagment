using AuthService.Application.RepositoryContract;
using AuthService.Application.Services.Contract;
using AuthService.Application.Services.Implementation;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure
{
    public class ServiceRegistration
    {
        public static void RegisteredServices(IServiceCollection services, IConfiguration configuration)
        {
            string ConnString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AuthDBContext>(options =>
            {
                options.UseSqlServer(ConnString);
            });

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            //Application Services
            services.AddScoped<IUserAppService, UserAppService>();
            //AutoMapper
            services.AddAutoMapper(typeof(Application.Mapper.UserMapper).Assembly);
        }
    }
}
