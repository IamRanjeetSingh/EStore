using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Models.UserModel;

namespace UserService.API.Requests.UserRequests.DeleteById
{
    internal sealed class DeleteByIdRequestHandler : IRequestHandler<DeleteByIdRequest, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DeleteByIdRequestHandler>? _logger;

        public DeleteByIdRequestHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, 
            ILogger<DeleteByIdRequestHandler>? logger = null)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteByIdRequest request, CancellationToken cancellationToken)
        {
            bool wasFound = await _userRepository.RemoveAsync(request.Id, cancellationToken);
            if(!wasFound)
            {
                _logger?.LogDebug("No User with id '{id}' found, returing 404 response.", request.Id);
                return new NotFoundObjectResult(new ErrorResponse(
                    _httpContextAccessor.HttpContext,
                    code: "USER_NOT_FOUND",
                    message: $"No User found with id '{request.Id}'"));
            }

            return new OkResult();
        }
    }
}
