using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Aggregates.Common
{
	public class Entity
	{
		public Id Id { get; }
		
		public Entity(Id id)
		{
			Id = id; 
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !obj.GetType().IsAssignableTo(typeof(Entity)))
				return false;
			return ((Entity)obj).Id == Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
