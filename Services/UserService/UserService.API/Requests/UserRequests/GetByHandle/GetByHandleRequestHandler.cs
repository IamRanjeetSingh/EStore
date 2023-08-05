using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Models.UserModel;
using SchemaModels = UserService.API.Schemas;

namespace UserService.API.Requests.UserRequests.GetByHandle
{
    internal sealed class GetByHandleRequestHandler : IRequestHandler<GetByHandleRequest, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetByHandleRequestHandler>? _logger;

        public GetByHandleRequestHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, 
            ILogger<GetByHandleRequestHandler>? logger = null)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetByHandleRequest request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(request.Handle, cancellationToken);
            if(user == null)
            {
                _logger?.LogDebug("No User with handle '{handle}' found, returning 404 response.", request.Handle);
                return new NotFoundObjectResult(new ErrorResponse(
                    _httpContextAccessor.HttpContext,
                    code: "USER_NOT_FOUND",
                    message: $"No User found by handle '{request.Handle}'."));
            }
            SchemaModels.User userSchema = CreateSchemaFromDomainModel(user);
            return new OkObjectResult(userSchema);
        }

        private SchemaModels.User CreateSchemaFromDomainModel(User user)
        {
            _logger?.LogDebug("Creating response schema from User.");
            return new SchemaModels.User(
                user.Id,
                user.Handle,
                new SchemaModels.Email(user.Email.Address, user.Email.IsVerified),
                new SchemaModels.Name(user.Name.FirstName, user.Name.LastName),
                user.DateOfBirth,
                user.CreationDate);
        }
    }
}
