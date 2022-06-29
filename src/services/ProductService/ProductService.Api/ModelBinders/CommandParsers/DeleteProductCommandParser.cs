using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using ProductService.Application.Commands;
using ProductService.Application.Commands.DeleteProduct;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProductService.Api.ModelBinders.CommandParsers
{
	internal sealed class DeleteProductCommandParser : CommandParser<DeleteProductCommand>
	{
		protected override async Task<DeleteProductCommand> ParseCommand(ModelBindingContext bindingContext)
		{
			HttpRequest httpRequest = bindingContext.HttpContext.Request;
			DeleteProductCommand cmd = new();
			cmd.Id = Guid.Parse((string)httpRequest.RouteValues["id"]);
			return cmd;
		}
	}
}
