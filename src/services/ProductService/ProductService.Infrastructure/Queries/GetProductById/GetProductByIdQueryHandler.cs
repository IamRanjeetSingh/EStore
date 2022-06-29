using ProductService.Application.Queries.GetProductById;
using ProductService.Application.Queries.GetProductById.Dto;
using ProductService.Infrastructure.Queries.TableInfo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Queries.GetProductById
{
	public sealed class GetProductByIdQueryHandler : IGetProductByIdQueryHandler
	{
		private readonly string connectionString;

		public GetProductByIdQueryHandler(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public async Task<IProductDto> Handle(GetProductByIdQuery qry)
		{
			using SqlConnection connection = new(connectionString);
			connection.Open();
			IProductDto product = await GetProduct(connection, qry);
			return product;
		}

		private async Task<IProductDto> GetProduct(SqlConnection connection, GetProductByIdQuery qry)
		{
			using SqlCommand cmd = connection.CreateCommand();
			cmd.CommandText = $@"
				SELECT [Product].[Id],
					[Product].[Title],
					[Product].[Description],
					[Product].[Price],
					[Product].[CreatedOn],
					[Property].[Name] AS PropertyName,
					[PropertyValue].[Value]
				FROM (SELECT [Id],[Title],[Description],[Price],[CreatedOn] FROM [Product] WHERE [Id] = @{ProductTableInfo.ColId}) AS Product 
				INNER JOIN [Property] AS Property
					ON [Product].[Id] = [Property].[ProductId]
				INNER JOIN [PropertyValue] AS PropertyValue 
					ON [Product].[Id] = [PropertyValue].[ProductId] AND [Property].[Name] = [PropertyValue].[PropertyName]
				ORDER BY [Property].[Name]";
			cmd.Parameters.AddWithValue($"@{ProductTableInfo.ColId}", qry.Id);

			using SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
			return await ParseProduct(dataReader);
		}

		private async Task<IProductDto> ParseProduct(SqlDataReader dataReader)
		{
			if (!await dataReader.ReadAsync())
				return null;

			ProductDto product = new();
			product.Id = Guid.Parse(dataReader[ProductTableInfo.ColId].ToString());
			product.Title = dataReader[ProductTableInfo.ColTitle].ToString();
			product.Description = dataReader[ProductTableInfo.ColDescription].ToString();
			product.Price = double.Parse(dataReader[ProductTableInfo.ColPrice].ToString());
			product.CreatedOn = DateTime.Parse(dataReader[ProductTableInfo.ColCreatedOn].ToString());

			product.Properties = await ParseProperties(dataReader);

			return product;
		}

		private async Task<IEnumerable<IPropertyDto>> ParseProperties(SqlDataReader dataReader)
		{
			Dictionary<string, List<string>> propertyNameValues = new();
			do
			{
				string propertyName = dataReader["PropertyName"].ToString();
				string propertyValue = dataReader[PropertyValueTableInfo.ColValue].ToString();

				if (!propertyNameValues.ContainsKey(propertyName))
					propertyNameValues.Add(propertyName, new List<string> { propertyValue });
				else
					propertyNameValues[propertyName].Add(propertyValue);
			} while (await dataReader.ReadAsync());

			List<IPropertyDto> properties = new();
			foreach (KeyValuePair<string, List<string>> nameValues in propertyNameValues)
			{
				PropertyDto property = new();
				property.Name = nameValues.Key;
				property.Values = nameValues.Value;

				properties.Add(property);
			}

			return properties;
		}
	}
}
