using CartService.Core.Models.CartModels;
using CartService.Core.Services;
using CartService.Infra.Utils;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CartService.Infra.Services
{
    internal sealed class ProductLookupService : IProductLookupService
    {
        private readonly IAPIAddress _productAPIAddress;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductLookupService>? _logger;

        public ProductLookupService(IAPIAddress productAPIAddress, IHttpClientFactory httpClientFactory, ILogger<ProductLookupService>? logger = null)
        {
            _productAPIAddress = productAPIAddress;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Product?> GetProductAsync(ProductId productId, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage getProductAPIResponse = await SendGetProductAPIRequestAsync(productId.Value);
            return CreateProductFromAPIResponseStatusCode(productId, getProductAPIResponse.StatusCode);
        }

        private Task<HttpResponseMessage> SendGetProductAPIRequestAsync(string productId, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Sending get product request to API for product id '{productId}'.", productId);

            HttpClient httpClient = _httpClientFactory.CreateClient();
            string sanitizedProductId = SanitizeProductId(productId);
            Uri requestUri = new(baseUri: _productAPIAddress.GetBaseAddress(), relativeUri: $"/product/{sanitizedProductId}");
            HttpRequestMessage getProductRequest = new(HttpMethod.Get, requestUri);
            return httpClient.SendAsync(getProductRequest, cancellationToken);
        }

        internal string SanitizeProductId(string productId)
        {
            //TODO: sanitize product id
            return productId;
        }

        private Product? CreateProductFromAPIResponseStatusCode(ProductId productId, HttpStatusCode apiResponseStatusCode)
        {
            if (apiResponseStatusCode != HttpStatusCode.OK)
            {
                _logger?.LogDebug("API response status code is {apiStatusCode}, assuming product doesn't exist.", (int)apiResponseStatusCode);
                _logger?.LogInformation("No Product found by id '{productId}'.", productId.Value);
                return null;
            }

            return new Product(productId);
        }
    }
}
