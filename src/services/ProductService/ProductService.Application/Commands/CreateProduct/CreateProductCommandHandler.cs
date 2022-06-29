using ProductService.Domain.Aggregates.Common;
using ProductService.Domain.Aggregates.ProductAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands.CreateProduct
{
	public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand,Guid>
	{
		private readonly DataContext dataContext;

		public CreateProductCommandHandler(DataContext dataContext)
		{
			this.dataContext = dataContext;
		}

		public Task<Guid> Handle(CreateProductCommand cmd)
		{
			if (cmd == null)
				throw new ArgumentNullException(nameof(cmd));

			return HandlePrivate(cmd);
		}

		private async Task<Guid> HandlePrivate(CreateProductCommand cmd)
		{
			PropertyCollection properties = new();
			foreach (PropertyCreateDetails propertyDetails in cmd.Properties)
				properties.Add(Property.Create(propertyDetails.Name, propertyDetails.Values));

			Product product = Product.Builder()
				.WithTitle(cmd.Title)
				.WithDescription(cmd.Description)
				.WithPrice(cmd.Price)
				.WithProperties(properties)
				.Build();

			Guid productId = await dataContext.ProductRepository.Add(product);
			await dataContext.UnitOfWork.SaveChanges();

			return productId;
		}
	}
}
