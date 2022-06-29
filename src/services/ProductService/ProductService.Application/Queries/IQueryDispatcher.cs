using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Queries
{
	public interface IQueryDispatcher
	{
		public Task<TResult> Send<TQuery,TResult>(TQuery qry) where TQuery : IQuery;
	}
}
