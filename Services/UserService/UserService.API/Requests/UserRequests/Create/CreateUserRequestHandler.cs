using EStore.Common.Extensions;
using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Controllers;
using UserService.Core.Exceptions;
using UserService.Core.Models.UserModel;
using SchemaModels = UserService.API.Schemas;

namespace UserService.API.Requests.UserRequests.Create
{
    internal sealed class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateUserRequestHandler>? _logger;

        public CreateUserRequestHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, 
            ILogger<CreateUserRequestHandler>? logger = null)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                User user = CreateUserFromArgs(request.Args);
                await _userRepository.AddAsync(user, cancellationToken);
                SchemaModels.User userSchema = CreateSchemaFromDomainModel(user);

                return new CreatedAtActionResult(
                    actionName: nameof(UserController.GetByHandle),
                    controllerName: nameof(UserController).RemoveSuffix("Controller"),
                    routeValues: new { userhandle = user.Handle.ToString() },
                    value: userSchema);   
            }
            catch(Exception ex) when (ex is ValueException || ex is UserServiceException)
            {
                _logger?.LogError("Error while creating Product, Exception: {ex}", ex);
                return new BadRequestObjectResult(new ErrorResponse(
                    _httpContextAccessor.HttpContext,
                    code: "BAD_REQUEST",
                    message: ex.Message));
            }
        }

        private User CreateUserFromArgs(SchemaModels.CreateUserArgs args)
        {
            _logger?.LogDebug("Creating new User from arguments.");
            User user = new(
                args.Handle,
                args.EmailAddress,
                args.Name.FirstName,
                args.Name.LastName,
                args.DateOfBirth);

            return user;
        }

        private SchemaModels.User CreateSchemaFromDomainModel(User user)
        {
            _logger?.LogDebug("Creating response schema from User.");
            SchemaModels.User userSchema = new(
                user.Id,
                user.Handle,
                new SchemaModels.Email(user.Email.Address, user.Email.IsVerified),
                new SchemaModels.Name(user.Name.FirstName, user.Name.LastName),
                user.DateOfBirth,
                user.CreationDate);

            return userSchema;
        }
    }
}
