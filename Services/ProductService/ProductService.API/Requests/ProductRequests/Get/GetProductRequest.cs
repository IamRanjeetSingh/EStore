using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Schemas;

namespace ProductService.API.Requests.ProductRequests.Get
{
    internal sealed class GetProductRequest : IRequest<IActionResult>
    {
        public Guid Id { get; }

        public GetProductRequest(Guid id)
        {
            Id = id;
        }
    }
}
