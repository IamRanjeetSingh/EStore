using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Queries
{
	public sealed class QueryDispatcher : IQueryDispatcher
	{
		private readonly IServiceProvider serviceProvider;

		public QueryDispatcher(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public Task<TResult> Send<TQuery, TResult>(TQuery qry) where TQuery : IQuery
		{
			IQueryHandler<TQuery, TResult> qryHandler = GetQueryHandler<TQuery, TResult>();
			if (qryHandler == null)
				throw new InvalidOperationException($"No {typeof(IQueryHandler<,>).FullName} found for query {typeof(TQuery).FullName} and result {typeof(TResult).FullName}.");

			return qryHandler.Handle(qry);
		}

		private IQueryHandler<TQuery,TResult> GetQueryHandler<TQuery,TResult>() where TQuery : IQuery
		{
			Type qryHandlerType = typeof(IQueryHandler<,>).MakeGenericType(typeof(TQuery), typeof(TResult));
			IQueryHandler<TQuery,TResult> qryHandler = (IQueryHandler<TQuery,TResult>)serviceProvider.GetService(qryHandlerType);
			return qryHandler;
		}
	}
}
