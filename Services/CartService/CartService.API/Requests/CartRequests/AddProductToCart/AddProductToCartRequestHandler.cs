using CartService.Core.Models.CartModels;
using CartService.Core.Services;
using EStore.Common.API.Schemas;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CartService.API.Requests.CartRequests.AddProductToCart
{
    public sealed class AddProductToCartRequestHandler : IRequestHandler<AddProductToCartRequest, IActionResult>
    {
        private readonly CartRepository _repository;
        private readonly IOwnerLookupService _ownerLookupService;
        private readonly IProductLookupService _productLookupService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AddProductToCartRequestHandler>? _logger;

        public AddProductToCartRequestHandler(CartRepository repository, IOwnerLookupService ownerLookupService, 
            IProductLookupService productLookupService, IHttpContextAccessor httpContextAccessor, 
            ILogger<AddProductToCartRequestHandler>? logger = null)
        {
            _repository = repository;
            _ownerLookupService = ownerLookupService;
            _productLookupService = productLookupService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(AddProductToCartRequest request, CancellationToken cancellationToken)
        {
            OwnerId? ownerId = GetOwnerId();
            if(ownerId == null)
                return OwnerIdNotFoundResponse();

            Owner? owner = await GetOwnerAsync(ownerId);
            if (owner == null)
                return OwnerNotFoundResponse(ownerId);

            Cart? cart = await GetCartForOwnerAsync(ownerId);
            if(cart == null)
                cart = CreateCartForOwner(owner);

            ProductId productId = new(request.ProductId);
            Product? product = await GetProductAsync(productId, cancellationToken);
            if(product == null)
                return ProductNotFoundResponse(productId);

            AddProductToCart(cart, product);
            await AddOrUpdateCartAsync(cart, cancellationToken);

            return new OkResult();
        }

        private OwnerId? GetOwnerId()
        {
            return new OwnerId("88a9d417-dfa0-477c-8805-e6a4abc1b75e");
            _logger?.LogInformation("Getting owner id from request.");

            OwnerId? ownerId = null;

            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                _logger?.LogDebug("No HttpContext available to retrieve owner id.");

            if(httpContext != null)
                ownerId = GetOwnerIdFromClaimsPrincipal(httpContext.User);

            return ownerId;
        }

        internal OwnerId? GetOwnerIdFromClaimsPrincipal(ClaimsPrincipal principal)
        {
            OwnerId? ownerId = null;

            Claim? idClaim = null;

            idClaim = principal.Claims.FirstOrDefault(claim => string.Equals(claim.Type, "id"));

            if (idClaim == null)
                _logger?.LogDebug("No Claim found for type '{idClaim}' to retrieve the owner id from.", "id");

            if (idClaim != null)
                ownerId = new(idClaim.Value);

            if (ownerId != null)
                _logger?.LogInformation("Owner id for request is '{ownerId}'", ownerId.Value);
            else
                _logger?.LogInformation("No owner id found in request.");

            return ownerId;
        }

        private async Task<Owner?> GetOwnerAsync(OwnerId ownerId)
        {
            _logger?.LogInformation("Getting Owner by id '{ownerId}'.", ownerId.Value);

            Owner? owner = await _ownerLookupService.GetOwnerAsync(ownerId);

            if (owner == null)
                _logger?.LogInformation("No Owner found by id '{ownerId}'.", ownerId.Value);

            return owner;
        }

        private async Task<Cart?> GetCartForOwnerAsync(OwnerId ownerId)
        {
            _logger?.LogInformation("Getting cart for owner id '{ownerId}'.", ownerId.Value);

            Cart? cart = await _repository.GetByOwnerIdAsync(ownerId);

            if (cart == null)
                _logger?.LogInformation("No cart found for owner id '{ownerId}'.", ownerId.Value);

            return cart;
        }

        private Cart CreateCartForOwner(Owner owner)
        {
            _logger?.LogInformation("Creating new cart for Owner with id '{ownerId}'.", owner.Id.Value);

            return new Cart(owner);
        }

        private async Task<Product?> GetProductAsync(ProductId productId, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Getting Product by id '{productId}'", productId.Value);

            Product? product = await _productLookupService.GetProductAsync(productId, cancellationToken);

            if (product == null)
                _logger?.LogInformation("No product found by id '{productId}'.", productId.Value);

            return product;
        }

        private void AddProductToCart(Cart cart, Product product)
        {
            _logger?.LogDebug("Adding product with id '{productId}' to cart with id '{cartId}'.", product.Id.Value, cart.Id.Value);

            cart.Products.Add(product);
        }

        private Task AddOrUpdateCartAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Adding or Updating cart with id '{cartId}' in repository.", cart.Id.Value);

            return _repository.AddOrUpdateAsync(cart, cancellationToken);
        }

        private BadRequestObjectResult OwnerIdNotFoundResponse()
        {
            ErrorResponse errorResponse = new(
                    _httpContextAccessor.HttpContext,
                    code: "OWNER_ID_MISSING",
                    message: "No owner id found for getting the cart.");
            return new BadRequestObjectResult(errorResponse);
        }

        private BadRequestObjectResult OwnerNotFoundResponse(OwnerId ownerId)
        {
            ErrorResponse errorResponse = new(
                _httpContextAccessor.HttpContext,
                code: "OWNER_NOT_FOUND",
                message: $"No owner found by id '{ownerId.Value}'.");
            return new BadRequestObjectResult(errorResponse);
        }

        private BadRequestObjectResult ProductNotFoundResponse(ProductId productId)
        {
            ErrorResponse errorResponse = new(
                _httpContextAccessor.HttpContext,
                code: "PRODUCT_NOT_FOUND",
                message: $"No product found by id '{productId.Value}'.");
            return new BadRequestObjectResult(errorResponse);
        }
    }
}
