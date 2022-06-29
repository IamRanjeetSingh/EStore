using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductService.Application.Queries;
using System;

namespace ProductService.Api.ModelBinders
{
	internal sealed class QueryModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context.Metadata.ModelType.IsAssignableTo(typeof(IQuery)))
				return new QueryModelBinder();

			return null;
		}
	}
}
