using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Infra.Utils
{
    internal sealed class StaticAPIAddress : IAPIAddress
    {
        private readonly Uri _baseAddress;

        public StaticAPIAddress(string baseAddress)
        {
            _baseAddress = new(baseAddress);    
        }

        public Uri GetBaseAddress()
        {
            return _baseAddress;
        }
    }
}
