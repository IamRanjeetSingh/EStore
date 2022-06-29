using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductService.Application.Commands;
using System;

namespace ProductService.Api.ModelBinders
{
	internal sealed class CommandModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context.Metadata.ModelType.IsAssignableTo(typeof(ICommand)))
				return new CommandModelBinder();

			return null;
		}
	}
}
