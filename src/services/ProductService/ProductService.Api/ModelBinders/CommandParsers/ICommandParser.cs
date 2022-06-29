using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductService.Application.Commands;
using System.Threading.Tasks;

namespace ProductService.Api.ModelBinders.CommandParsers
{
	public interface ICommandParser
	{
		public Task<ICommand> Parse(ModelBindingContext bindingContext);
	}
}
