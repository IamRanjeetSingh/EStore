using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductService.Core.Models.ProductModel;
using ProductService.Core.Models.TemplateModel;
using ProductService.Infrastructure.Models.ProductModel.MongoDb;
using ProductService.Infrastructure.Models.TemplateModel.MongoDb;

namespace ProductService.Infrastructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
        {
            AddTemplateModelDependencies(services, config);
            AddProductModelDependencies(services, config);
            return services;
        }

        private static void AddTemplateModelDependencies(IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITemplateRepository, TemplateMongoDbRepository>();
            services.Configure<TemplateMongoDbDAO.Options>(config.GetRequiredSection("Database:MongoDb"));
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IOptionsSnapshot<TemplateMongoDbDAO.Options>>().Value);
            services.AddScoped<ITemplateMongoDbDAO, TemplateMongoDbDAO>();
        }

        private static void AddProductModelDependencies(IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IProductRepository, ProductMongoDbRepository>();
            services.Configure<ProductMongoDbDAO.Options>(config.GetRequiredSection("Database:MongoDb"));
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IOptionsSnapshot<ProductMongoDbDAO.Options>>().Value);
            services.AddScoped<IProductMongoDbDAO, ProductMongoDbDAO>();
        }
    }
}
