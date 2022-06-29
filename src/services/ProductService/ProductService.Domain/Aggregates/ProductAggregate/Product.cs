using ProductService.Domain.Aggregates.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.ProductAggregate
{
	public class Product : AggregateRoot
	{
		public Title Title { get; private set; }
		public Description Description { get; private set; }
		public Price Price { get; private set; }
		public DateTime CreatedOn { get; private set; }
		public PropertyCollection Properties { get; set; }

		protected internal Product(Id id, Title title, Description description, Price price, DateTime createdOn, 
			PropertyCollection properties) : base(id)
		{
			Title = title;
			Description = description;
			Price = price;
			CreatedOn = createdOn;
			Properties = properties;
		}

		public static ProductBuilder.ITitleStep Builder()
		{
			return new ProductBuilder.StepBuilder();
		}

		public void UpdateTitle(Title title)
		{
			Title = title;
		}

		public void UpdateDescription(Description description)
		{
			Description = description;
		}

		public void UpdatePrice(Price price)
		{
			Price = price;
		}

		public void UpdateProperties(PropertyCollection properties)
		{
			Properties = properties;
		}
	}
}
