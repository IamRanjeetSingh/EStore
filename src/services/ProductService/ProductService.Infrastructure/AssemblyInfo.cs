using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("ProductService.Application.Tests")]
[assembly: InternalsVisibleTo("ProductService.Infrastructure.Tests")]
namespace ProductService.Infrastructure
{
	public sealed class AssemblyInfo
	{
	}
}
