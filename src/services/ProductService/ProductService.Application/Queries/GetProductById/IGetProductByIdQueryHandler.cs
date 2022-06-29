using ProductService.Application.Queries.GetProductById.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Queries.GetProductById
{
	public interface IGetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, IProductDto>
	{
		
	}
}
