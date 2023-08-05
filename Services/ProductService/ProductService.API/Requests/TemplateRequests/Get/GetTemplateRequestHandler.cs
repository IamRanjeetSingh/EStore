using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Schemas;
using ProductService.Core.Models.TemplateModel;
using CoreModels = ProductService.Core.Models.TemplateModel;
using SchemaModels = ProductService.API.Schemas;

namespace ProductService.API.Requests.TemplateRequests.Get
{
    public sealed class GetTemplateRequestHandler : IRequestHandler<GetTemplateRequest, IActionResult>
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetTemplateRequestHandler>? _logger;

        public GetTemplateRequestHandler(ITemplateRepository templateRepository, IHttpContextAccessor httpContextAccessor,
            ILogger<GetTemplateRequestHandler>? logger = null)
        {
            _templateRepository = templateRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetTemplateRequest request, CancellationToken cancellationToken)
        {
            CoreModels.Template? template = await _templateRepository.GetAsync(request.Id, cancellationToken);
            if (template == null)
            {
                _logger?.LogDebug("No Template found by id '{id}', returning 404 response.", request.Id);
                return new NotFoundObjectResult(new ErrorResponse(
                    _httpContextAccessor.HttpContext,
                    code: "TEMPLATE_NOT_FOUND",
                    message: $"No Template found by id '{request.Id}'."));
            }

            SchemaModels.Template? templateSchema = CreateTemplateSchema(template);

            return new OkObjectResult(templateSchema);
        }

        private SchemaModels.Template? CreateTemplateSchema(CoreModels.Template template)
        {
            _logger?.LogDebug("Creating response schema from template.");

            SchemaModels.Template templateSchema = new(
                id: template.Id,
                name: template.Name,
                properties: template.Properties.Select(property => new SchemaModels.TemplateProperty(
                    name: property.Name)));
            return templateSchema;
        }
    }
}
