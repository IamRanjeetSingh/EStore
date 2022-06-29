using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using ProductService.Application.Queries;
using ProductService.Application.Queries.GetProductById;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProductService.Api.ModelBinders.QueryParsers
{
	internal sealed class GetProductByIdQueryParser : QueryParser<GetProductByIdQuery>
	{
		protected override Task<GetProductByIdQuery> ParseQuery(ModelBindingContext bindingContext)
		{
			GetProductByIdQuery qry = new();
			qry.Id = Guid.Parse((string)bindingContext.HttpContext.Request.RouteValues["id"]);
			return Task.FromResult(qry);
		}
	}
}
