using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductService.Application.Commands;
using System.Threading.Tasks;

namespace ProductService.Api.ModelBinders.CommandParsers
{
	public abstract class CommandParser<TCommand> : ICommandParser where TCommand : ICommand
	{
		public async Task<ICommand> Parse(ModelBindingContext bindingContext)
		{
			return await ParseCommand(bindingContext);
		}

		protected abstract Task<TCommand> ParseCommand(ModelBindingContext bindingContext);
	}
}
