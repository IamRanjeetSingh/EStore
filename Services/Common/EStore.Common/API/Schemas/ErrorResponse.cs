using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace EStore.Common.API.Schemas
{
    public sealed class ErrorResponse
    {
        [JsonPropertyName("code")]
        public string? Code { get; }

        [JsonPropertyName("message")]
        public string Message { get; }

        [JsonPropertyName("errors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Errors { get; }

        public ErrorResponse(HttpContext? httpContext, string? code, string message, string[]? errors = null)
        {
            Code = code;
            Message = message;
            Errors = errors;
        }
    }
}
