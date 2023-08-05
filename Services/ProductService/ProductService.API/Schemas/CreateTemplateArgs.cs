using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ProductService.API.Schemas
{
    public sealed class CreateTemplateArgs
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; }

        [Required]
        [JsonPropertyName("properties")]
        public IEnumerable<TemplateProperty> Properties { get; }

        public CreateTemplateArgs(string name, IEnumerable<TemplateProperty> properties)
        {
            Name = name;
            Properties = properties;
        }
    }
}
