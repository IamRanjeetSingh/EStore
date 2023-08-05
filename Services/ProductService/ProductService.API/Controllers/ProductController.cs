using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Requests.ProductRequests.Create;
using ProductService.API.Requests.ProductRequests.Delete;
using ProductService.API.Requests.ProductRequests.Get;
using ProductService.API.Schemas;
using ProductService.Core.Exceptions;

namespace ProductService.API.Controllers
{
    /// <summary>
    /// Operations for Product.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController>? _logger;

        public ProductController(IMediator mediator, ILogger<ProductController>? logger = null)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Create a new Product.
        /// </summary>
        /// <param name="createProductArgs">Request arguments to create a new Product.</param>
        /// <returns>API action result.</returns>
        /// <response code="201" type="">Newly created Product.</response>
        /// <response code="400">Bad request.</response>
        [HttpPost("")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Create([FromBody] CreateProductArgs createProductArgs)
        {
            CreateProductRequest request = new(createProductArgs);

            _logger?.LogDebug("Sending {requestName} to RequestHandler.", nameof(CreateProductRequest));
            return _mediator.Send(request);
        }

        /// <summary>
        /// Get Product by id.
        /// </summary>
        /// <param name="productId">Id of the Product.</param>
        /// <returns>API action result.</returns>
        /// <response code="200">Product with matching id.</response>
        /// <response code="404">No Product found with matching id.</response>
        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetById(Guid productId)
        {
            GetProductRequest request = new(productId);

            _logger?.LogDebug("Sending {requestName} to RequestHandler.", nameof(GetProductRequest));
            return _mediator.Send(request);
        }

        /// <summary>
        /// Delete Product by id.
        /// </summary>
        /// <param name="productId">Id of the Product.</param>
        /// <returns>API action result.</returns>
        /// <response code="200">Product with matching id deleted.</response>
        /// <response code="404">No Product found with matching id.</response>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public Task<IActionResult> DeleteById([FromRoute] Guid productId)
        {
            DeleteProductRequest request = new(productId);

            _logger?.LogDebug("Sending {requestName} to RequestHandler.", nameof(DeleteProductRequest));
            return _mediator.Send(request);
        }
    }
}
