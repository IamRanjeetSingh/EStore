using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories
{
	public sealed class SqlUnitOfWork : ISqlUnitOfWork
	{
		private readonly SqlConnection connection;
		private SqlTransaction transaction;

		public SqlUnitOfWork(string connectionString)
		{
			connection = new(connectionString);
			connection.Open();
			transaction = connection.BeginTransaction();
		}

		public void Dispose()
		{
			transaction.Dispose();
			connection.Dispose();
		}

		public SqlConnection GetConnection()
		{
			return connection;
		}

		public SqlTransaction GetTransaction()
		{
			return transaction;
		}

		public async Task SaveChanges()
		{
			await transaction.CommitAsync();
			transaction.Dispose();
			transaction = connection.BeginTransaction();
		}
	}
}
