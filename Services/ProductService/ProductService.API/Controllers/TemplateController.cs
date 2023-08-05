using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Requests.TemplateRequests.Create;
using ProductService.API.Requests.TemplateRequests.Delete;
using ProductService.API.Requests.TemplateRequests.Get;
using ProductService.API.Schemas;
using EStore.Common.API.Schemas;

namespace ProductService.API.Controllers
{
    //TODO: Add Summary for swagger
    //TODO: Try Add sample request body for swagger
    //TODO: Try add sample response body for swagger
    /// <summary>
    /// Operations for Template.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public sealed class TemplateController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TemplateController>? _logger;

        public TemplateController(IMediator mediator, ILogger<TemplateController>? logger = null)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Create a new Template.
        /// </summary>
        /// <param name="createTemplateArgs">Request arguments to create a new Template.</param>
        /// <returns>API action result.</returns>
        /// <response code="201" type="">Newly created Template.</response>
        /// <response code="400">Bad request.</response>
        [HttpPost("")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Template), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Create([FromBody] CreateTemplateArgs createTemplateArgs)
        {
            CreateTemplateRequest request = new(createTemplateArgs);

            _logger?.LogDebug("Sending {requestName} to RequestHandler.", nameof(CreateTemplateRequest));
            //CreatedAtActionResult result = (CreatedAtActionResult)await _mediator.Send(request);
            //return CreatedAtAction(result.ActionName, result.RouteValues, result.Value);
            return _mediator.Send(request);
        }

        /// <summary>
        /// Get Template by id.
        /// </summary>
        /// <param name="templateId">Id of the Template.</param>
        /// <returns>API action result.</returns>
        /// <response code="200">Template with matching id.</response>
        /// <response code="404">No Template found with matching id.</response>
        [HttpGet("{templateId}")]
        [ProducesResponseType(typeof(Template), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetById([FromRoute] Guid templateId)
        {
            GetTemplateRequest request = new(templateId);

            _logger?.LogDebug("Sending {requestName} to RequestHandler.", nameof(GetTemplateRequest));
            return _mediator.Send(request);
        }

        /// <summary>
        /// Delete Template by id.
        /// </summary>
        /// <param name="templateId">Id of the Template.</param>
        /// <returns>API action result.</returns>
        /// <response code="200">Template with matching id deleted.</response>
        /// <response code="404">No Template found with matching id.</response>
        [HttpDelete("{templateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public Task<IActionResult> DeleteById([FromRoute] Guid templateId)
        {
            DeleteTemplateRequest request = new(templateId);

            _logger?.LogDebug("Sending {requestName} to RequestHandler.", nameof(DeleteTemplateRequest));
            return _mediator.Send(request);
        }
    }
}
