using ProductService.Domain.Aggregates.Common;
using ProductService.Domain.Aggregates.ProductAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands.UpdateProduct
{
	public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand,bool>
	{
		private readonly DataContext dataContext;

		public UpdateProductCommandHandler(DataContext dataContext)
		{
			this.dataContext = dataContext;
		}

		public Task<bool> Handle(UpdateProductCommand cmd)
		{
			if (cmd == null)
				throw new ArgumentNullException(nameof(cmd));

			return HandleImpl(cmd);
		}

		private async Task<bool> HandleImpl(UpdateProductCommand cmd)
		{
			Product product = await dataContext.ProductRepository.Get(cmd.Id);
			if (product == null)
				return false;

			product.UpdateTitle(cmd.Title);
			product.UpdateDescription(cmd.Description);
			product.UpdatePrice(cmd.Price);

			PropertyCollection properties = new();
			foreach (PropertyUpdateDetails propUpdDetails in cmd.Properties)
				properties.Add(Property.Create(propUpdDetails.Name, propUpdDetails.Values));

			if (cmd.Properties.Any())
				product.UpdateProperties(properties);

			bool isUpdateSuccessful = await dataContext.ProductRepository.Update(product);
			await dataContext.UnitOfWork.SaveChanges();

			return isUpdateSuccessful;
		}
	}
}
