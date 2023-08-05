using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.API.Requests.TemplateRequests.Create
{
    public sealed class CreateTemplateRequest : IRequest<IActionResult>
    {
        public Schemas.CreateTemplateArgs Args { get; }

        public CreateTemplateRequest(Schemas.CreateTemplateArgs args)
        {
            Args = args;
        }
    }
}
