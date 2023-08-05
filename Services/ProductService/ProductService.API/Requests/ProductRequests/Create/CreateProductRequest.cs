using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Schemas;

namespace ProductService.API.Requests.ProductRequests.Create
{
    public sealed class CreateProductRequest : IRequest<IActionResult>
    {
        public CreateProductArgs Args { get; }

        public CreateProductRequest(CreateProductArgs args)
        {
            Args = args;
        }
    }
}
