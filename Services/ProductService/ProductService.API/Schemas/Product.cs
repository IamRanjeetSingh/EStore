using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductService.API.Schemas
{
    public sealed class Product
    {
        [Required]
        [JsonPropertyName("id")]
        public Guid Id { get; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; }

        [Required]
        [JsonPropertyName("templateId")]
        public Guid TemplateId { get; }

        [Required]
        [JsonPropertyName("properties")]
        public IEnumerable<ProductProperty> Properties { get; }

        public Product(Guid id, string name, Guid templateId, IEnumerable<ProductProperty> properties)
        {
            Id = id;
            Name = name;
            TemplateId = templateId;
            Properties = properties;
        }
    }

    public sealed class ProductProperty
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; }

        [Required]
        [JsonPropertyName("value")]
        public string Value { get; }

        public ProductProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
