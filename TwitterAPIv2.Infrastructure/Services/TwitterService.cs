using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
using TwitterAPIv2.Infrastructure.Utilities;
using static Flurl.Url;

namespace TwitterAPIv2.Infrastructure.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TwitterAuthSettings> _settings;
        private readonly ILogger<TwitterService> _logger;
        private readonly ITwitterAuthApi _twitterAuthApi;
        private readonly ICalculateMetrics _calcMetrics;
        private static readonly byte[] data =  Encoding.ASCII.GetBytes("This is a test string to demonstrate range requests");

        public TwitterService(HttpClient httpClient, IOptions<TwitterAuthSettings> settings, ITwitterAuthApi twitterAuthApi, ICalculateMetrics calcMetrics, ILogger<TwitterService> logger)
        {
            _httpClient = httpClient;
            _settings = settings;
            _twitterAuthApi = twitterAuthApi;
            _calcMetrics = calcMetrics;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyCollection<TweetStreamResponse>, Error>> GetSampleTweets()
        {
            //var token = _twitterAuthApi.FetchToken(_settings.Value.ClientId, _settings.Value.ClientSecret);
            var token = _settings.Value.BearerToken;
            var uri = new Uri(Combine(_settings.Value.BaseUrl, "/tweets/sample/stream?tweet.fields=public_metrics,entities"));

            _httpClient.Timeout = TimeSpan.FromMinutes(2);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            string response = null;
            var tweetStreamResponse = new List<TweetStreamResponse>();

            try
            {
                using (var stream = await _httpClient.GetStreamAsync(uri))
                {
                    var hashtags = new List<string>();
                    var topHashTags = new Dictionary<string, int>();
                    Console.WriteLine("Top 10 hashtags");

                    var reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        response = reader.ReadLine();
                        //_logger.LogInformation($"Tweet Stream Response: { response }", response);
                        tweetStreamResponse.Add(JsonConvert.DeserializeObject<TweetStreamResponse>(response));
                        
                        dynamic dynObj = JsonConvert.DeserializeObject(response);
                        if (dynObj.data.entities.hashtags != null)
                        {
                            foreach (var htag in dynObj.data.entities.hashtags)
                            {
                                string tag = htag.tag;
                                hashtags.Add(tag);
                                //Determine top hashtags
                                _calcMetrics.TopHashTags(hashtags, topHashTags);
                            }
                        }
                    }
                    stream.Close();
                    return Result.Success<IReadOnlyCollection<TweetStreamResponse>, Error>(tweetStreamResponse);
                }
            }
            catch(Exception ex)
            {
                return Result.Failure<IReadOnlyCollection<TweetStreamResponse>, Error>(
                        Errors.TweetManagement.StreamError(ex.StackTrace));
            }
        }
    }
}
