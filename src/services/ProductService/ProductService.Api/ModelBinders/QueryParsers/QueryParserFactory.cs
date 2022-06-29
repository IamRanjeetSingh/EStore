using System;

namespace ProductService.Api.ModelBinders.QueryParsers
{
	public sealed class QueryParserFactory : IQueryParserFactory
	{
		private readonly IServiceProvider serviceProvider;

		public QueryParserFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public IQueryParser GetParserForType(Type qryType)
		{
			Type qryParserType = typeof(QueryParser<>).MakeGenericType(qryType);
			IQueryParser qryParser = (IQueryParser)serviceProvider.GetService(qryParserType);
			return qryParser;
		}
	}
}
