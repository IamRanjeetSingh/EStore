using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductService.Api.ModelBinders.QueryParsers;
using ProductService.Application.Queries;
using System;
using System.Threading.Tasks;

namespace ProductService.Api.ModelBinders
{
	internal sealed class QueryModelBinder : IModelBinder
	{
		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (!bindingContext.ModelType.IsAssignableTo(typeof(IQuery)))
				return;

			IServiceProvider serviceProvider = bindingContext.HttpContext.RequestServices;
			IQueryParserFactory qryParserFactory = (IQueryParserFactory)serviceProvider.GetService(typeof(IQueryParserFactory));
			IQueryParser qryParser = qryParserFactory.GetParserForType(bindingContext.ModelType);
			if (qryParser == null)
			{
				bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"No {typeof(IQueryParser).FullName} was found.");
				return;
			}

			bindingContext.Result = ModelBindingResult.Success(await qryParser.Parse(bindingContext));
		}
	}
}
