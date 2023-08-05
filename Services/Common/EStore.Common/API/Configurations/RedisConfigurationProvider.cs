using Microsoft.Extensions.Configuration;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Common.API.Configurations
{
    internal sealed class RedisConfigurationProvider : ConfigurationProvider
    {
        public override void Load()
        {
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("redis-appsettings:6379");
            IDatabase database = connection.GetDatabase();
            //Data.Add(database.StringGet("UserServiceApi"));
            Data.Add();
        }
    }
}
