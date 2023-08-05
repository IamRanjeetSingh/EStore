using ProductService.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Core.Services.DomainGateway
{
    internal sealed class DomainGatewayService : IDomainGatewayService
    {
        public Task<bool> DoesDomainExist(Guid domainId, CancellationToken cancellationToken = default)
        {
            //TODO: Implement call to DomainService
            return Task.FromResult(true);
        }
    }
}
