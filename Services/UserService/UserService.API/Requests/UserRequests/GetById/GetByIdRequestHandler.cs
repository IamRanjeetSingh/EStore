using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Models.UserModel;
using SchemaModels = UserService.API.Schemas;

namespace UserService.API.Requests.UserRequests.GetById
{
    internal sealed class GetByIdRequestHandler : IRequestHandler<GetByIdRequest, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetByIdRequestHandler>? _logger;

        public GetByIdRequestHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, 
            ILogger<GetByIdRequestHandler>? logger = null)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(request.Id, cancellationToken);
            if(user == null)
            {
                _logger?.LogDebug("No User found by id '{id}', returning 404 response.", request.Id);
                return new NotFoundObjectResult(new ErrorResponse(
                    _httpContextAccessor.HttpContext,
                    code: "USER_NOT_FOUND",
                    message: $"No User found by id '{request.Id}'."));
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
