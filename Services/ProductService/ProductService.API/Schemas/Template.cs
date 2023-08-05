using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductService.API.Schemas
{
    public sealed class Template
    {
        [Required]
        [JsonPropertyName("id")]
        public Guid Id { get; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; }

        [Required]
        [JsonPropertyName("properties")]
        public IEnumerable<TemplateProperty> Properties { get; }

        public Template(Guid id, string name, IEnumerable<TemplateProperty> properties)
        {
            Id = id;
            Name = name;
            Properties = properties;
        }
    }

    public sealed class TemplateProperty
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; }

        public TemplateProperty(string name)
        {
            Name = name;
        }
    }
}
