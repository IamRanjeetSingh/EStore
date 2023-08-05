using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CartService.API.Requests.CartRequests.AddProductToCart
{
    public sealed class AddProductToCartRequest : IRequest<IActionResult>
    {
        public string ProductId { get; }

        public AddProductToCartRequest(string productId)
        {
            ProductId = productId;
        }
    }
}
