using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories.ProductRepositoryNS.Dao.PersistentModels
{
	public sealed class PersistentProperty
	{
		internal string Name { get; set; }
		internal Guid ProductId { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null || !obj.GetType().IsAssignableTo(typeof(PersistentProperty)))
				return false;

			PersistentProperty persistentProperty = (PersistentProperty)obj;

			if (persistentProperty.Name != Name)
				return false;
			if (persistentProperty.ProductId != ProductId)
				return false;
			return true;
		}

		public override int GetHashCode()
		{
			return (Name + ProductId.ToString()).GetHashCode();
		}
	}
}
