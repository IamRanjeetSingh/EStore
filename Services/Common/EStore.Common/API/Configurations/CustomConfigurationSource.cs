using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Common.API.Configurations
{
    internal sealed class CustomConfigurationSource : IConfigurationSource
    {
        private readonly Func<IConfigurationProvider> _configurationProviderAccessor;

        public CustomConfigurationSource(Func<IConfigurationProvider> configurationProviderAccessor)
        {
            _configurationProviderAccessor = configurationProviderAccessor;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return _configurationProviderAccessor.Invoke();
        }
    }
}
