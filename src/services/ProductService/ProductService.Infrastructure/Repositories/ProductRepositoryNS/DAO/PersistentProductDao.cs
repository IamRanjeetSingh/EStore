using ProductService.Infrastructure.Exceptions;
using ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao
{
	public sealed class PersistentProductDao : IPersistentProductDao
	{
		private const string Table = "Product";
		private const string ColId = "Id";
		private const string ColTitle = "Title";
		private const string ColDescription = "Description";
		private const string ColPrice = "Price";
		private const string ColCreatedOn = "CreatedOn";
		private const string ColRowVersion = "RowVersion";

		private readonly ISqlUnitOfWork _unitOfWork;

		public PersistentProductDao(ISqlUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Guid> Insert(PersistentProduct persistentProduct)
		{
			SqlCommand cmd = BuildInsertCommand(persistentProduct);
			await cmd.ExecuteNonQueryAsync();
			return persistentProduct.Id;
		}

		private SqlCommand BuildInsertCommand(PersistentProduct persistentProduct)
		{
			string cmdText = $@"
			INSERT INTO 
				{Table}(
					{ColId},
					{ColTitle},
					{ColDescription},
					{ColPrice},
					{ColCreatedOn},
					{ColRowVersion})
			VALUES(
				@{ColId},
				@{ColTitle},
				@{ColDescription},
				@{ColPrice},
				@{ColCreatedOn},
				@{ColRowVersion})";

			SqlCommand cmd = new(cmdText, _unitOfWork.GetConnection(), _unitOfWork.GetTransaction());
			
			cmd.Parameters.AddWithValue($"@{ColId}", persistentProduct.Id);
			cmd.Parameters.AddWithValue($"@{ColTitle}", persistentProduct.Title);
			cmd.Parameters.AddWithValue($"@{ColDescription}", persistentProduct.Description);
			cmd.Parameters.AddWithValue($"@{ColPrice}", persistentProduct.Price);
			cmd.Parameters.AddWithValue($"@{ColCreatedOn}", persistentProduct.CreatedOn);
			cmd.Parameters.AddWithValue($"@{ColRowVersion}", Guid.NewGuid());

			return cmd;
		}

		public async Task<PersistentProduct> Get(Guid id)
		{
			SqlCommand cmd = BuildGetCommand(id);
			SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
			PersistentProduct persistentProduct = ParseGetResult(dataReader);
			dataReader.Close();
			return persistentProduct;
		}

		private SqlCommand BuildGetCommand(Guid id)
		{
			string cmdText = $@"
			SELECT 
				{ColId},
				{ColTitle},
				{ColDescription},
				{ColPrice},
				{ColCreatedOn}
			FROM
				{Table}
			WHERE 
				{ColId} = @{ColId}";

			SqlCommand cmd = new(cmdText, _unitOfWork.GetConnection(), _unitOfWork.GetTransaction());

			cmd.Parameters.AddWithValue($"@{ColId}", id);

			return cmd;
		}

		private PersistentProduct ParseGetResult(SqlDataReader dataReader)
		{
			if (!dataReader.HasRows)
				return null;

			dataReader.Read();

			PersistentProduct persistentProduct = new();
			persistentProduct.Id = Guid.Parse(dataReader[ColId].ToString());
			persistentProduct.Title = dataReader[ColTitle].ToString();
			persistentProduct.Description = dataReader[ColDescription].ToString();
			persistentProduct.Price = double.Parse(dataReader[ColPrice].ToString());
			persistentProduct.CreatedOn = DateTime.Parse(dataReader[ColCreatedOn].ToString());

			return persistentProduct;
		}

		public async Task<bool> Update(PersistentProduct persistentProduct)
		{
			Guid currentRowVersion = await GetRowVersionOfProduct(persistentProduct.Id);
			if (currentRowVersion == Guid.Empty)
				return false;

			SqlCommand cmd = BuildUpdateCommand(persistentProduct, currentRowVersion);
			int rowsAffected = await cmd.ExecuteNonQueryAsync();

			if (rowsAffected == 0)
				throw new DbUpdateConcurrencyException(currentRowVersion);

			return true;
		}

		private SqlCommand BuildUpdateCommand(PersistentProduct persistentProduct, Guid currentRowVersion)
		{
			string cmdText = $@"
			UPDATE 
				{Table}
			SET
				{ColTitle} = @{ColTitle},
				{ColDescription} = @{ColDescription},
				{ColPrice} = @{ColPrice},
				{ColRowVersion} = @New_{ColRowVersion}
			WHERE
				{ColId} = @{ColId} AND
				{ColRowVersion} = @Current_{ColRowVersion}";

			SqlCommand cmd = new(cmdText, _unitOfWork.GetConnection(), _unitOfWork.GetTransaction());

			cmd.Parameters.AddWithValue($"@{ColId}", persistentProduct.Id);
			cmd.Parameters.AddWithValue($"@{ColTitle}", persistentProduct.Title);
			cmd.Parameters.AddWithValue($"@{ColDescription}", persistentProduct.Description);
			cmd.Parameters.AddWithValue($"@{ColPrice}", persistentProduct.Price);
			cmd.Parameters.AddWithValue($"@Current_{ColRowVersion}", currentRowVersion);
			cmd.Parameters.AddWithValue($"@New_{ColRowVersion}", Guid.NewGuid());

			return cmd;
		}
		
		public async Task<bool> Delete(Guid id)
		{
			Guid currentRowVersion = await GetRowVersionOfProduct(id);
			if (currentRowVersion == Guid.Empty)
				return false;

			SqlCommand cmd = BuildDeleteCommand(id, currentRowVersion);
			int rowsAffected = await cmd.ExecuteNonQueryAsync();

			if (rowsAffected == 0)
				throw new DbUpdateConcurrencyException(currentRowVersion);

			return true;
		}

		private SqlCommand BuildDeleteCommand(Guid id, Guid currentRowVersion)
		{
			string cmdText = $@"
			DELETE FROM
				{Table}
			WHERE
				{ColId} = @{ColId} AND
				{ColRowVersion} = @{ColRowVersion}";

			SqlCommand cmd = new(cmdText, _unitOfWork.GetConnection(), _unitOfWork.GetTransaction());

			cmd.Parameters.AddWithValue($"@{ColId}", id);
			cmd.Parameters.AddWithValue($"@{ColRowVersion}", currentRowVersion);

			return cmd;
		}

		private async Task<Guid> GetRowVersionOfProduct(Guid productId)
		{
			string cmdText = $@"SELECT {ColRowVersion} FROM {Table} WHERE {ColId} = @{ColId}";
			SqlCommand cmd = new(cmdText, _unitOfWork.GetConnection(), _unitOfWork.GetTransaction());
			cmd.Parameters.AddWithValue($"@{ColId}", productId);

			SqlDataReader dataReader = await cmd.ExecuteReaderAsync();

			if (!dataReader.HasRows)
			{
				dataReader.Close();
				return Guid.Empty;
			}

			dataReader.Read();

			Guid rowVersion = Guid.Parse(dataReader[ColRowVersion].ToString());
			dataReader.Close();

			return rowVersion;
		}
	}
}
