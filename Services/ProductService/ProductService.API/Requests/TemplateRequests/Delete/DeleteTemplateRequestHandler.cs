using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Schemas;
using ProductService.Core.Models.TemplateModel;

namespace ProductService.API.Requests.TemplateRequests.Delete
{
    public class DeleteTemplateRequestHandler : IRequestHandler<DeleteTemplateRequest, IActionResult>
    {
        private readonly ITemplateRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DeleteTemplateRequestHandler> _logger;

        public DeleteTemplateRequestHandler(ITemplateRepository templateRepository, IHttpContextAccessor httpContextAccessor,
            ILogger<DeleteTemplateRequestHandler> logger)
        {
            _repository = templateRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteTemplateRequest request, CancellationToken cancellationToken)
        {
            bool wasFound = await _repository.RemoveAsync(request.Id, cancellationToken);
            if (!wasFound)
            {
                _logger?.LogDebug("No Template found by id '{id}', returning 404 response.", request.Id);
                return new NotFoundObjectResult(new ErrorResponse(
                    _httpContextAccessor.HttpContext,
                    code: "TEMPLATE_NOT_FOUND",
                    message: $"No Template found by id '{request.Id}'."));
            }

            return new OkResult();
        }
    }
}
