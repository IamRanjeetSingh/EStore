using ProductService.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Core.Services.DomainGateway
{
    internal interface IDomainGatewayService
    {
        public Task<bool> DoesDomainExist(Guid domainId, CancellationToken cancellationToken = default);
    }
}
