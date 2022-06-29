using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductService.Api.Exceptions;
using ProductService.Api.ModelBinders.CommandParsers;
using ProductService.Application.Commands;
using System;
using System.Threading.Tasks;

namespace ProductService.Api.ModelBinders
{
	internal sealed class CommandModelBinder : IModelBinder
	{
		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (!bindingContext.ModelType.IsAssignableTo(typeof(ICommand)))
				return;

			IServiceProvider serviceProvider = bindingContext.HttpContext.RequestServices;
			ICommandParserFactory cmdParserFactory = (ICommandParserFactory)serviceProvider.GetService(typeof(ICommandParserFactory));
			ICommandParser cmdParser = cmdParserFactory.GetParserForType(bindingContext.ModelType);
			if (cmdParser == null)
			{
				bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"No {typeof(ICommandParser).FullName} was found.");
				return;
			}

			try
			{
				bindingContext.Result = ModelBindingResult.Success(await cmdParser.Parse(bindingContext));
			}
			catch(InvalidCommandException e)
			{
				bindingContext.ModelState.AddModelError(bindingContext.ModelName, e.Message);
			}
		}
	}
}
