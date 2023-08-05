namespace ProductService.Core.Exceptions
{
    public class AuthorizationException : AccessException
    {
        public AuthorizationException(string? message) : base(message) { }
    }
}
