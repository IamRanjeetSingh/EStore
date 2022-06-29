using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels
{
	public sealed class PersistentPropertyValue
	{
		internal string PropertyName { get; set; }
		internal Guid ProductId { get; set; }
		internal string Value { get; set; }
		internal int Count { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null || !obj.GetType().IsAssignableTo(typeof(PersistentPropertyValue)))
				return false;

			PersistentPropertyValue persistentPropertyValue = (PersistentPropertyValue)obj;

			if (persistentPropertyValue.PropertyName != PropertyName)
				return false;
			if (persistentPropertyValue.ProductId != ProductId)
				return false;
			if (persistentPropertyValue.Value != Value)
				return false;
			if (persistentPropertyValue.Count != Count)
				return false;
			return true;
		}

		public override int GetHashCode()
		{
			return (PropertyName + ProductId.ToString()).GetHashCode();
		}
	}
}
