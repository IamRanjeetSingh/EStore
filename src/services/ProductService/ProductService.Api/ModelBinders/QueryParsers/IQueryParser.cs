using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductService.Application.Queries;
using System.Threading.Tasks;

namespace ProductService.Api.ModelBinders.QueryParsers
{
	public interface IQueryParser
	{
		public Task<IQuery> Parse(ModelBindingContext bindingContext);
	}
}
