using CartService.API.Requests.CartRequests.AddProductToCart;
using EStore.Common.API.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CartService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ControllerExceptionFilter]
    public sealed class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CartController>? _logger;

        public CartController(IMediator mediator, ILogger<CartController>? logger = null)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("product/{productId}")]
        public Task<IActionResult> AddProductToCart([FromRoute] string productId)
        {
            AddProductToCartRequest request = new(productId);
            _logger?.LogDebug("Sending request '{requestName}' to handler.", nameof(AddProductToCartRequest));
            return _mediator.Send(request);
        }
    }
}
