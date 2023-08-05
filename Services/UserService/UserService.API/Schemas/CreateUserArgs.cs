using System.Text.Json.Serialization;
using UserService.API.Utils.JsonSerialization;

namespace UserService.API.Schemas
{
    public sealed class CreateUserArgs
    {
        [JsonPropertyName("handle")]
        public string Handle { get; }

        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; }

        [JsonPropertyName("name")]
        public Name Name { get; }

        [JsonPropertyName("dateOfBirth")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly DateOfBirth { get; }

        public CreateUserArgs(string handle, string emailAddress, Name name, DateOnly dateOfBirth)
        {
            Handle = handle;
            EmailAddress = emailAddress;
            Name = name;
            DateOfBirth = dateOfBirth;
        }
    }
}
