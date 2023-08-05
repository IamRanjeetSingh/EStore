using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Schemas;

namespace ProductService.API.Requests.TemplateRequests.Get
{
    public sealed class GetTemplateRequest : IRequest<IActionResult>
    {
        public Guid Id { get; }

        public GetTemplateRequest(Guid id)
        {
            Id = id;
        }
    }
}
