using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Domain.Aggregates.ProductAggregate;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels
{
	public sealed class PersistentProduct
	{
		private const int PricePrecision = Domain.Aggregates.ProductAggregate.Price.Precision;

		private double price;
		private DateTime createdOn;

		internal Guid Id { get; set; }
		internal string Title { get; set; }
		internal string Description { get; set; }
		internal double Price { get => price; set => price = Math.Round(value, PricePrecision); }
		internal DateTime CreatedOn 
		{ 
			get => createdOn; 
			set => createdOn = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Kind); 
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !obj.GetType().IsAssignableTo(typeof(PersistentProduct)))
				return false;

			PersistentProduct product = (PersistentProduct)obj;
			
			if (product.Id != Id)
				return false;
			if (product.Title != Title)
				return false;
			if (product.Description != Description)
				return false;
			if (product.Price != Price)
				return false;
			if (product.CreatedOn.CompareTo(CreatedOn) != 0)
				return false;

			return true;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
