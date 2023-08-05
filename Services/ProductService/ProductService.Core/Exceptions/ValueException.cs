namespace ProductService.Core.Exceptions
{
    public class ValueException : ProductServiceException
    {
        public ValueException(string? message) : base(message) { }
    }
}
