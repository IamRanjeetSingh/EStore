using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Infra.Utils
{
    internal interface IAPIAddress
    {
        public Uri GetBaseAddress();
    }
}
