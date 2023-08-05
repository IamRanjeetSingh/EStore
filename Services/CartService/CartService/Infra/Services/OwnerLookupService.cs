using CartService.Core.Models.CartModels;
using CartService.Core.Services;
using CartService.Infra.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CartService.Infra.Services
{
    internal sealed class OwnerLookupService : IOwnerLookupService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAPIAddress _userAPILocation;
        private readonly ILogger<OwnerLookupService>? _logger;

        public OwnerLookupService(IAPIAddress userAPILocation, IHttpClientFactory httpClientFactory, ILogger<OwnerLookupService>? logger = null) 
        {
            _httpClientFactory = httpClientFactory;
            _userAPILocation = userAPILocation;
            _logger = logger;
        }

        public async Task<Owner?> GetOwnerAsync(OwnerId id)
        {
            HttpResponseMessage apiResponse = await SendGetUserAPIRequestAsync(id.Value);
            Owner? owner;
            if (apiResponse.StatusCode == HttpStatusCode.OK)
            {
                owner = new(id);
            }
            else if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                _logger?.LogInformation("Owner not found.");
                owner = null;
            }
            else
            {
                _logger?.LogError("Unexpected status code {statusCode} from user api while getting user by id {userId}", apiResponse.StatusCode, id.Value);
                owner = null;
            }

            return owner;
        }

        internal Task<HttpResponseMessage> SendGetUserAPIRequestAsync(string userId, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Sending get user request to API for user id '{userId}'.", userId);

            HttpClient httpClient = _httpClientFactory.CreateClient();
            string sanitizedUserId = SanitizeUserId(userId);
            Uri requestUri = new(baseUri: _userAPILocation.GetBaseAddress(), relativeUri: $"/user/{sanitizedUserId}");
            HttpRequestMessage getUserRequest = new(HttpMethod.Get, requestUri);
            return httpClient.SendAsync(getUserRequest, cancellationToken);
            
        }

        internal string SanitizeUserId(string userId)
        {
            //TODO: sanitize user id
            return userId;
        }
    }
}
