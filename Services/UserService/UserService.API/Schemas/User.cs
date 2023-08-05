using System.Text.Json.Serialization;
using UserService.API.Utils.JsonSerialization;

namespace UserService.API.Schemas
{
    public sealed class User
    {
        [JsonPropertyName("id")]
        public Guid Id { get; }

        [JsonPropertyName("handle")]
        public string Handle { get; }

        [JsonPropertyName("email")]
        public Email Email { get; }

        [JsonPropertyName("name")]
        public Name Name { get; }

        [JsonPropertyName("dateOfBirth")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly DateOfBirth { get; }

        [JsonPropertyName("creationDate")]
        public DateTimeOffset CreationDate { get; }

        public User(Guid id, string handle, Email email, Name name, DateOnly dateOfBirth, DateTimeOffset creationDate)
        {
            Id = id;
            Handle = handle;
            Email = email;
            Name = name;
            DateOfBirth = dateOfBirth;
            CreationDate = creationDate;
        }
    }

    public sealed class Email
    {
        public string Address { get; }
        public bool IsVerified { get; }

        public Email(string address, bool isVerified)
        {
            Address = address;
            IsVerified = isVerified;
        }
    }

    public sealed class Name
    {
        public string FirstName { get; }
        public string LastName { get; }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
