using ProductService.Domain.Aggregates.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories
{
	public interface ISqlUnitOfWork : IUnitOfWork
	{
		public SqlConnection GetConnection();
		public SqlTransaction GetTransaction();
	}
}
