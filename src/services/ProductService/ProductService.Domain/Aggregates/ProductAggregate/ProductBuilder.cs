using ProductService.Domain.Aggregates.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.ProductAggregate
{
	public static class ProductBuilder
	{
		internal sealed class StepBuilder : ITitleStep, IDescriptionStep, IPriceStep, IPropertiesStep, IBuildStep
		{
			private Title _title;
			private Description _description;
			private Price _price;
			private PropertyCollection _properties;

			public IDescriptionStep WithTitle(Title title)
			{
				_title = title;
				return this;
			}

			public IPriceStep WithDescription(Description description)
			{
				_description = description;
				return this;
			}

			public IPropertiesStep WithPrice(Price price)
			{
				_price = price;
				return this;
			}

			public IBuildStep WithProperties(PropertyCollection properties)
			{
				_properties = properties;
				return this;
			}

			public Product Build()
			{
				Id id = Guid.NewGuid();
				DateTime createdOn = DateTime.UtcNow;
				return new Product(id, _title, _description, _price, createdOn, _properties);
			}
		}

		public interface ITitleStep
		{
			public IDescriptionStep WithTitle(Title title);
		}

		public interface IDescriptionStep
		{
			public IPriceStep WithDescription(Description description);
		}

		public interface IPriceStep
		{
			public IPropertiesStep WithPrice(Price price);
		}

		public interface IPropertiesStep
		{
			public IBuildStep WithProperties(PropertyCollection properties);
		}

		public interface IBuildStep
		{
			public Product Build();
		}
	}
}
