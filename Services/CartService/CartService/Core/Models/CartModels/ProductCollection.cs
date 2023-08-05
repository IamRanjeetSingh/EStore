using CartService.Core.Exceptions;
using CartService.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Core.Models.CartModels
{
    public sealed class ProductCollection : IEnumerable<Product>
    {
        private readonly LinkedList<Product> _products;

        internal ProductCollection() : this(Array.Empty<Product>()) { }

        internal ProductCollection(IEnumerable<Product> products)
        {
            _products = new(products);
        }

        public void Add(Product product)
        {
            _products.AddLast(product);
        }

        public bool RemoveByProductId(ProductId productId)
        {
            Product? product = _products.FirstOrDefault(product => product.Id.Equals(productId));
            if (product == null)
                return false;
            return _products.Remove(product);
        }

        public IEnumerator<Product> GetEnumerator()
        {
            return _products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
