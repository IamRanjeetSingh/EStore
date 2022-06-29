using ProductService.Domain.Aggregates.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Commands.DeleteProduct
{
	public sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand,bool>
	{
		private readonly DataContext dataContext;

		public DeleteProductCommandHandler(DataContext dataContext)
		{
			this.dataContext = dataContext;
		}

		public Task<bool> Handle(DeleteProductCommand cmd)
		{
			if (cmd == null)
				throw new ArgumentNullException(nameof(cmd));

			return HandleImpl(cmd);
		}

		private async Task<bool> HandleImpl(DeleteProductCommand cmd)
		{
			bool isDeleteSuccessful = await dataContext.ProductRepository.Remove(cmd.Id);
			await dataContext.UnitOfWork.SaveChanges();

			return isDeleteSuccessful;
		}
	}
}
