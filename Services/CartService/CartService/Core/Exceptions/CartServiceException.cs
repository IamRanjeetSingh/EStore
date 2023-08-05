using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Core.Exceptions
{
    public class CartServiceException : Exception
    {
        public CartServiceException(string message) : base(message) { }
    }
}
