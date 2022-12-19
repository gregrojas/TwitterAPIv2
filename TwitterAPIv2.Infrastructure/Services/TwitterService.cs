using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using TwitterAPIv2.Core.Errors;
using TwitterAPIv2.Infrastructure.Responses;
using static Flurl.Url;

namespace TwitterAPIv2.Infrastructure.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TwitterAuthSettings> _settings;
        private readonly ILogger<TwitterService> _logger;
        private readonly ITwitterAuthApi _twitterAuthApi;
        private static readonly byte[] data =  Encoding.ASCII.GetBytes("This is a test string to demonstrate range requests");

        public TwitterService(HttpClient httpClient, IOptions<TwitterAuthSettings> settings, ITwitterAuthApi twitterAuthApi, ILogger<TwitterService> logger)
        {
            _httpClient = httpClient;
            _settings = settings;
            _twitterAuthApi = twitterAuthApi;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyCollection<TweetStreamResponse>, Error>> GetSampleTweets()
        {
            //var token = _twitterAuthApi.FetchToken(_settings.Value.ClientId, _settings.Value.ClientSecret);
            var token = _settings.Value.BearerToken;
            var uri = new Uri(Combine(_settings.Value.BaseUrl, "/tweets/sample/stream?tweet.fields=public_metrics"));

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri)
            {
                Headers =
                {
                    { "Authorization", $"Bearer {token}" }
                }
            };

            _httpClient.Timeout = TimeSpan.FromMinutes(2);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            //var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);        

            string response = null;
            var tweetStreamResponse = new List<TweetStreamResponse>();
            
            //using (var stream = await response.Content.ReadAsStreamAsync())
            using (var stream = await _httpClient.GetStreamAsync(uri))
            {
                StreamReader reader = new StreamReader(stream);
                while(!reader.EndOfStream)
                {
                    response = reader.ReadLine();
                    _logger.LogInformation("TweetStreamResponse: { response }", response);
                    tweetStreamResponse.Add(JsonConvert.DeserializeObject<TweetStreamResponse>(response));

                    //Calculate metrics; determine top hashtags

                }
                stream.Close();
                return Result.Success<IReadOnlyCollection<TweetStreamResponse>, Error>(null);
            }

            //if (response.StatusCode != HttpStatusCode.OK)
            //    return Result.Failure<IReadOnlyCollection<TweetStreamResponse>, Error>(
            //        Errors.TweetManagement.StreamError(response.StatusCode.ToString()));

            //var tweetStreamResponse = JsonConvert.DeserializeObject<IReadOnlyCollection<TweetStreamResponse>>(await response.Content.ReadAsStringAsync());
            
        }
    }
}
