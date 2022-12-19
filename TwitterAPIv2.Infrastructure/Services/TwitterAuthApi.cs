using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitterAPIv2.Infrastructure.Responses;
using TwitterAPIv2.Infrastructure.Utilities;

namespace TwitterAPIv2.Infrastructure.Services
{
    public class TwitterAuthApi : ITwitterAuthApi
    {
        private readonly HttpClient _httpClient;
        private readonly TwitterAuthSettings _settings;
        private readonly ILogger<TwitterAuthApi> _logger;

        public TwitterAuthApi(HttpClient httpClient, IOptions<TwitterAuthSettings> settings, ILogger<TwitterAuthApi> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        public string FetchToken(string clientId, string clientSecret)
        {
            var tokenFromService = GetAuthTokenFromServiceAsync(clientId, clientSecret);
            var accessToken = tokenFromService.Result.AccessToken;
            return accessToken;
        }

        private async Task<OAuthTokenResponse> GetAuthTokenFromServiceAsync(string clientId, string clientSecret)
        {
            var uri = new Uri(_settings.AuthUrl);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", clientId },
                    { "client_secret", clientSecret }
                })
            };

            var response = await _httpClient.SendAsync(requestMessage);

            var parsedResponse =
                SerializerHelper.DeserializeFromSnakeCase<OAuthTokenResponse>(
                    await response.Content.ReadAsStringAsync());

            return parsedResponse;
        }
    }
}
