namespace ProductService.Core.Exceptions
{
    public class AuthenticationException : AccessException
    {
        public AuthenticationException(string? message) : base(message) { }
    }
}
