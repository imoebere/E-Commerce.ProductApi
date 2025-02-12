
using E_Commerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // Add database connectivity
            // Add Authentication scheme
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FileName"]!);

            // Create a Dependency Injection (DI)
            services.AddScoped<IProduct, ProductRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrasturePolicy(this IApplicationBuilder app)
        {
            /*Register middleware such as:
             * Global Exception: Handle external errors
             * Listen to Api Gateways: which blocks outside calls. 
            */
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
