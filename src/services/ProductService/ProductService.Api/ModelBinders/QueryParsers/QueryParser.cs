using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductService.Application.Queries;
using System.Threading.Tasks;

namespace ProductService.Api.ModelBinders.QueryParsers
{
	public abstract class QueryParser<TQuery> : IQueryParser where TQuery : IQuery
	{
		public async Task<IQuery> Parse(ModelBindingContext bindingContext)
		{
			return await ParseQuery(bindingContext);
		}

		protected abstract Task<TQuery> ParseQuery(ModelBindingContext bindingContext);
	}
}
