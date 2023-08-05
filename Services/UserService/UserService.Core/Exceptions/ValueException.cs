namespace UserService.Core.Exceptions
{
    public class ValueException : UserServiceException
    {
        public ValueException(string? message) : base(message) { }
    }
}
