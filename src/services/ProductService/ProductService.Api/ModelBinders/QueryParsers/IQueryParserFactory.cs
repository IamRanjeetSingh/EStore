using System;

namespace ProductService.Api.ModelBinders.QueryParsers
{
	public interface IQueryParserFactory
	{
		public IQueryParser GetParserForType(Type qryType);
	}
}
