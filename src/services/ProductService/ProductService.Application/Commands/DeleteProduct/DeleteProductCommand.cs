using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands.DeleteProduct
{
	public sealed class DeleteProductCommand : ICommand
	{
		public Guid Id { get; set; }
	}
}
