using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.API.Requests.TemplateRequests.Delete
{
    public sealed class DeleteTemplateRequest : IRequest<IActionResult>
    {
        public Guid Id { get; }

        public DeleteTemplateRequest(Guid id)
        {
            Id = id;
        }
    }
}
