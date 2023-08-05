using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Requests.UserRequests.Create;
using UserService.API.Requests.UserRequests.DeleteById;
using UserService.API.Requests.UserRequests.GetByHandle;
using UserService.API.Requests.UserRequests.GetById;
using UserService.API.Schemas;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController>? _logger;

        public UserController(IMediator mediator, ILogger<UserController>? logger = null)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUserArgs args)
        {
            CreateUserRequest request = new(args);

            _logger?.LogDebug("Sending {request} to RequestHandler.", nameof(CreateUserRequest));
            return await _mediator.Send(request);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid userId)
        {
            GetByIdRequest request = new(userId);

            _logger?.LogDebug("Sending {request} to RequestHandler.", nameof(GetByIdRequest));
            return await _mediator.Send(request);
        }

        [HttpGet("handle/{userHandle}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByHandle([FromRoute] string userHandle)
        {
            GetByHandleRequest request = new(userHandle);

            _logger?.LogDebug("Sending {request} to RequestHandler.", nameof(GetByHandleRequest));
            return await _mediator.Send(request);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid userId)
        {
            DeleteByIdRequest request = new(userId);

            _logger?.LogDebug("Sending request {request} to RequestHandler.", nameof(DeleteByIdRequest));
            return await _mediator.Send(request);
        }
    }
}
