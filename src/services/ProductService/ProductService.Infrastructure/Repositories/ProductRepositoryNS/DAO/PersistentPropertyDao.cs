using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao
{
	public sealed class PersistentPropertyDao : IPersistentPropertyDao
	{
		private const string Table = "Property";
		private const string ColName = "Name";
		private const string ColProductId = "ProductId";


		private readonly ISqlUnitOfWork _unitOfWork;

		public PersistentPropertyDao(ISqlUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<PersistentProperty>> GetByProduct(Guid productId)
		{
			SqlCommand cmd = BuildGetByProductCommand(productId);
			SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
			IEnumerable<PersistentProperty> persistentProperties = await ParseGetByProductResult(dataReader);
			dataReader.Close();
			return persistentProperties;
		}

		private SqlCommand BuildGetByProductCommand(Guid productId)
		{
			string cmdText = $@"
			SELECT 
				{ColName},
				{ColProductId}
			FROM
				{Table}
			WHERE
				{ColProductId} = @{ColProductId}
			ORDER BY
				{ColName}";

			SqlCommand cmd = new(cmdText, _unitOfWork.GetConnection(), _unitOfWork.GetTransaction());

			cmd.Parameters.AddWithValue($"@{ColProductId}", productId);

			return cmd;
		}

		private async Task<IEnumerable<PersistentProperty>> ParseGetByProductResult(SqlDataReader dataReader)
		{
			List<PersistentProperty> persistentProperties = new();

			if (!dataReader.HasRows)
				return persistentProperties;
			
			while(await dataReader.ReadAsync())
			{
				PersistentProperty persistentProperty = new();
				persistentProperty.ProductId = Guid.Parse(dataReader[ColProductId].ToString());
				persistentProperty.Name = dataReader[ColName].ToString();

				persistentProperties.Add(persistentProperty);
			}

			return persistentProperties;
		}

		public async Task Merge(Guid productId, IEnumerable<PersistentProperty> persistentProperties)
		{
			SqlCommand cmd = BuildMergeCommand(productId, persistentProperties);
			await cmd.ExecuteNonQueryAsync();
		}

		private SqlCommand BuildMergeCommand(Guid productId, IEnumerable<PersistentProperty> persistentProperties)
		{
			StringBuilder values = new();
			List<SqlParameter> parameters = new();
			for (int index = 0; index < persistentProperties.Count(); index++)
			{
				values.Append(
					$"(@{ColName}_{index}, @{ColProductId}_{index}),");

				PersistentProperty persistentProperty = persistentProperties.ElementAt(index);
				parameters.Add(new($"@{ColName}_{index}", persistentProperty.Name));
				parameters.Add(new($"@{ColProductId}_{index}", persistentProperty.ProductId));
			}
			if (values.Length > 0)
			{
				values.Remove(values.Length - 1, 1);
				values.Insert(0, "VALUES ");
			}
			else
				values.Append($"SELECT * FROM [{Table}] WHERE 1 = 0");

			string cmdText = $@"
			WITH [PropertySubset] AS (
				SELECT 
					[{ColName}], 
					[{ColProductId}] 
				FROM
					[{Table}]
				WHERE [{ColProductId}] = @{ColProductId})
			MERGE 
				INTO [PropertySubset] AS [T] 
				USING ({values}) AS [S]([{ColName}], [{ColProductId}])
				ON ([S].[{ColName}] = [T].[{ColName}] AND [S].[{ColProductId}] = [T].[{ColProductId}])
			WHEN NOT MATCHED BY TARGET THEN
				INSERT([{ColName}], [{ColProductId}]) 
				VALUES ([S].[{ColName}], [S].[{ColProductId}])
			WHEN MATCHED THEN
				UPDATE SET
					[T].[{ColName}] = [S].[{ColName}],
					[T].[{ColProductId}] = [S].[{ColProductId}]
			WHEN NOT MATCHED BY SOURCE THEN
				DELETE;";

			SqlCommand cmd = new(cmdText, _unitOfWork.GetConnection(), _unitOfWork.GetTransaction());
			cmd.Parameters.AddRange(parameters.ToArray());
			cmd.Parameters.AddWithValue($"@{ColProductId}", productId);

			return cmd;
		}
	}
}
