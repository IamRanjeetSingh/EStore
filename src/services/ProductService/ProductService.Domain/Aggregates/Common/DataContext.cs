using ProductService.Domain.Aggregates.ProductAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.Common
{
	public sealed class DataContext
	{
		public IUnitOfWork UnitOfWork { get; }
		public ProductRepository ProductRepository { get; }

		public DataContext(IUnitOfWork unitOfWork, ProductRepository productRepository)
		{
			UnitOfWork = unitOfWork;
			ProductRepository = productRepository;
		}
	}
}
