using CartService.Core.Models.CartModels;
using CartService.Core.Services;
using CartService.Infra.Database.CartModels;
using CartService.Infra.Services;
using CartService.Infra.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CartService
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCartService(this IServiceCollection services, IConfiguration config)
        {
            AddCoreLayer(services);
            AddInfraLayer(services, config.GetRequiredSection("Infra"));
            return services;
        }

        private static void AddCoreLayer(IServiceCollection services)
        {
            services.AddSingleton<CartRepository>();
        }

        private static void AddInfraLayer(IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient();
            services.AddSingleton<ICartDAO, CartMongoDbDAO>();
            services.Configure<CartMongoDbDAO.Options>(config.GetRequiredSection("Database:Cart:MongoDb"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<CartMongoDbDAO.Options>>().Value);
            services.AddSingleton<IOwnerLookupService, OwnerLookupService>(sp => 
                new OwnerLookupService(
                    new StaticAPIAddress(config.GetRequiredSection("API:User:BaseAddress").Value!), 
                    sp.GetRequiredService<IHttpClientFactory>(), 
                    sp.GetService<ILogger<OwnerLookupService>>()));
            services.AddSingleton<IProductLookupService, ProductLookupService>(sp =>
                new ProductLookupService(
                    new StaticAPIAddress(config.GetRequiredSection("API:Product:BaseAddress").Value!),
                    sp.GetRequiredService<IHttpClientFactory>(),
                    sp.GetService<ILogger<ProductLookupService>>()));
        }
    }
}
