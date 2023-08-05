using Microsoft.Extensions.DependencyInjection;
using ProductService.Core.Services.ProductBuider;

namespace ProductService.Core.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCoreLayer(this IServiceCollection services)
        {
            RegisterServices(services);
            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ProductBuilderService>();
        }
    }
}
