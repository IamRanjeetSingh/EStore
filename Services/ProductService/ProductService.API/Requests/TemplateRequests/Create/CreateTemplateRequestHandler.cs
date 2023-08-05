using EStore.Common.Extensions;
using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Controllers;
using ProductService.API.Schemas;
using ProductService.Core.Exceptions;
using ProductService.Core.Models.TemplateModel;
using CoreModels = ProductService.Core.Models.TemplateModel;
using SchemaModels = ProductService.API.Schemas;

namespace ProductService.API.Requests.TemplateRequests.Create
{
    public sealed class CreateTemplateRequestHandler : IRequestHandler<CreateTemplateRequest, IActionResult>
    {
        private readonly ITemplateRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateTemplateRequestHandler>? _logger;

        public CreateTemplateRequestHandler(ITemplateRepository repository, IHttpContextAccessor httpContextAccessor,
            ILogger<CreateTemplateRequestHandler>? logger = null)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(CreateTemplateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                CoreModels.Template template = CreateTemplate(request.Args);
                await _repository.AddAsync(template, cancellationToken);
                SchemaModels.Template templateSchema = CreateTemplateSchema(template);

                return new CreatedAtActionResult(
                    actionName: nameof(TemplateController.GetById),
                    controllerName: nameof(TemplateController).RemoveSuffix("Controller"),
                    routeValues: new { templateId = templateSchema.Id },
                    value: templateSchema);
            }
            catch (Exception ex) when (ex is ValueException || ex is ProductServiceException || ex is FormatException)
            {
                _logger?.LogError("Error while creating Template, Exception: \n{ex},\nreturning 400 response.", ex);
                return new BadRequestObjectResult(new ErrorResponse(
                    _httpContextAccessor.HttpContext,
                    code: "BAD_REQUEST",
                    message: ex.Message));
            }
        }

        private CoreModels.Template CreateTemplate(CreateTemplateArgs args)
        {
            _logger?.LogDebug("Creating new Template from arguments.");

            CoreModels.Template template = new(
                name: args.Name,
                properties: args.Properties.Select(propertyArgs => new CoreModels.TemplateProperty(
                    name: propertyArgs.Name)));
            return template;
        }

        private SchemaModels.Template CreateTemplateSchema(CoreModels.Template template)
        {
            _logger?.LogDebug("Creating response schema from Template.");
            SchemaModels.Template templateSchema = new(
                id: template.Id,
                name: template.Name,
                properties: template.Properties.Select(property => new SchemaModels.TemplateProperty(
                    name: property.Name)));
            return templateSchema;
        }
    }
}
