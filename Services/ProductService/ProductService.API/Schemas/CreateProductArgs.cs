using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductService.API.Schemas
{
    public sealed class CreateProductArgs
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; }

        [Required]
        [JsonPropertyName("templateId")]
        public Guid TemplateId { get; }

        [Required]
        [JsonPropertyName("properties")]
        public IEnumerable<ProductProperty> Properties { get; }

        public CreateProductArgs(string name, Guid templateId, IEnumerable<ProductProperty> properties)
        {
            Name = name;
            TemplateId = templateId;
            Properties = properties;
        }
    }
}
