namespace UserService.Core.Exceptions
{
    public class AccessException : UserServiceException
    {
        public AccessException(string? message) : base(message) { }
    }
}
