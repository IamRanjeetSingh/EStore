using ProductService.Core.Models.Common;

namespace ProductService.Core.Models.TemplateModel
{
    public sealed class TemplateProperty : ValueObject
    {
        public Name Name { get; }

        public TemplateProperty(string name)
        {
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is TemplateProperty property)
                return string.Equals(property.Name, Name);
            return false;
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Name);
            return hashCode.ToHashCode();
        }
    }
}
