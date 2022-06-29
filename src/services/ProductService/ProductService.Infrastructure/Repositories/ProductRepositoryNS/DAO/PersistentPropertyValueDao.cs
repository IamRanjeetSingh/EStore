using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao
{
	public sealed class PersistentPropertyValueDao : IPersistentPropertyValueDao
	{
		private const string Table = "PropertyValue";
		private const string ColPropertyName = "PropertyName";
		private const string ColProductId = "ProductId";
		private const string ColValue = "Value";
		private const string ColCount = "Count";

		private readonly ISqlUnitOfWork unitOfWork;

		public PersistentPropertyValueDao(ISqlUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<PersistentPropertyValue>> GetByProduct(Guid productId)
		{
			SqlCommand cmd = BuildGetByProductCommand(productId);
			SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
			IEnumerable<PersistentPropertyValue> propertyValues = await ParseGetByProductResult(dataReader);
			dataReader.Close();
			return propertyValues;
		}

		private SqlCommand BuildGetByProductCommand(Guid productId)
		{
			string cmdText = $@"
			SELECT 
				[{ColPropertyName}],
				[{ColProductId}],
				[{ColValue}],
				[{ColCount}]
			FROM
				[{Table}]
			WHERE
				[{ColProductId}] = @{ColProductId}";

			SqlCommand cmd = new(cmdText, unitOfWork.GetConnection(), unitOfWork.GetTransaction());
			cmd.Parameters.AddWithValue($"@{ColProductId}", productId);

			return cmd;
		}

		private async Task<IEnumerable<PersistentPropertyValue>> ParseGetByProductResult(SqlDataReader dataReader)
		{
			List<PersistentPropertyValue> persistentPropertyValues = new();

			if (!dataReader.HasRows)
				return persistentPropertyValues;

			while (await dataReader.ReadAsync())
			{
				PersistentPropertyValue persistentPropertyValue = new();
				persistentPropertyValue.PropertyName = dataReader[ColPropertyName].ToString();
				persistentPropertyValue.ProductId = Guid.Parse(dataReader[ColProductId].ToString());
				persistentPropertyValue.Value = dataReader[ColValue].ToString();
				persistentPropertyValue.Count = int.Parse(dataReader[ColCount].ToString());

				persistentPropertyValues.Add(persistentPropertyValue);
			}

			return persistentPropertyValues;

		}

		public async Task Merge(Guid productId, IEnumerable<PersistentPropertyValue> persistentPropertyValues)
		{
			SqlCommand cmd = BuildMergeCommand(productId, persistentPropertyValues);
			await cmd.ExecuteNonQueryAsync();
		}

		private SqlCommand BuildMergeCommand(Guid productId, IEnumerable<PersistentPropertyValue> persistentPropertyValues)
		{
			StringBuilder values = new();
			List<SqlParameter> parameters = new();
			for (int index = 0; index < persistentPropertyValues.Count(); index++)
			{
				values.Append($"(@{ColPropertyName}_{index}, @{ColProductId}_{index}, @{ColValue}_{index}, @{ColCount}_{index}),");

				PersistentPropertyValue persistentPropertyValue = persistentPropertyValues.ElementAt(index);
				parameters.Add(new($"@{ColPropertyName}_{index}", persistentPropertyValue.PropertyName));
				parameters.Add(new($"@{ColProductId}_{index}", persistentPropertyValue.ProductId));
				parameters.Add(new($"@{ColValue}_{index}", persistentPropertyValue.Value));
				parameters.Add(new($"@{ColCount}_{index}", persistentPropertyValue.Count));
			}
			if (values.Length > 0)
			{
				values.Remove(values.Length - 1, 1);
				values.Insert(0, "VALUES ");
			}
			else
				values.Append($"SELECT * FROM [{Table}] WHERE 1 = 0");

			string cmdText = $@"
			WITH [PropertyValueSubset] AS (
				SELECT 
					[{ColPropertyName}],
					[{ColProductId}],
					[{ColValue}],
					[{ColCount}]
				FROM
					[{Table}]
				WHERE
					[{ColProductId}] = @{ColProductId})
			MERGE 
				INTO [PropertyValueSubset] AS [T]
				USING ({values}) AS [S]([{ColPropertyName}], [{ColProductId}], [{ColValue}], [{ColCount}])
				ON (
					[S].[{ColPropertyName}] = [T].[{ColPropertyName}] AND 
					[S].[{ColProductId}] = [T].[{ColProductId}] AND 
					[S].[{ColValue}] = [T].[{ColValue}])
			WHEN NOT MATCHED BY TARGET THEN
				INSERT(
					[{ColPropertyName}], 
					[{ColProductId}], 
					[{ColValue}],
					[{ColCount}]) 
				VALUES(
					[S].[{ColPropertyName}], 
					[S].[{ColProductId}], 
					[S].[{ColValue}],
					[S].[{ColCount}])
			WHEN MATCHED THEN
				UPDATE SET
					[T].[{ColCount}] = [S].[{ColCount}]
			WHEN NOT MATCHED BY SOURCE THEN
				DELETE;";

			SqlCommand cmd = new(cmdText, unitOfWork.GetConnection(), unitOfWork.GetTransaction());
			cmd.Parameters.AddRange(parameters.ToArray());
			cmd.Parameters.AddWithValue($"@{ColProductId}", productId);

			return cmd;
		}
	}
}
