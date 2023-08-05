using System.Reflection;

namespace UserService.Core.Models
{
    public abstract class ValueObject
    {
        public abstract override bool Equals(object? obj);

        public abstract override int GetHashCode();
    }
}
