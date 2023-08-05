using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Core.Models.UserModel;
using UserService.Infrastructure.Models.UserModel.MongoDb;
using UserService.Infrastructure.Utils;

namespace UserService.Infrastructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
        {
            AddRepository(services, config);
            return services;
        }

        private static void AddRepository(IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IMongoDbClientFactory, MongoDbClientFactory>();
            services.Configure<UserMongoDbDao.Options>(config.GetRequiredSection("Database:MongoDb"));
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IOptionsSnapshot<UserMongoDbDao.Options>>().Value);
            services.AddScoped<UserMongoDbDao>();
            services.AddScoped<IUserRepository, UserMongoDbRepository>();
        }
    }
}
