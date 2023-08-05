using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.API.Requests.ProductRequests.Delete
{
    internal sealed class DeleteProductRequest : IRequest<IActionResult>
    {
        public Guid Id { get; }

        public DeleteProductRequest(Guid id)
        {
            Id = id;
        }
    }
}
