using CartService.Core.Models.CartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Core.Services
{
    public interface IProductLookupService
    {
        public Task<Product?> GetProductAsync(ProductId productId, CancellationToken cancellationToken = default);
    }
}
