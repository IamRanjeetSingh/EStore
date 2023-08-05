using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Core.Exceptions
{
    public class ValueException : CartServiceException
    {
        public ValueException(string message) : base(message) { }
    }
}
