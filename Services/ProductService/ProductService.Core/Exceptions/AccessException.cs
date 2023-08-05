namespace ProductService.Core.Exceptions
{
    public class AccessException : ProductServiceException
    {
        public AccessException(string? message) : base(message) { }
    }
}
