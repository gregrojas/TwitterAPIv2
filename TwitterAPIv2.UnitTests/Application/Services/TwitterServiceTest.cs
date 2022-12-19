using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TwitterAPIv2.Infrastructure.Responses;
using TwitterAPIv2.Infrastructure.Services;
using TwitterAPIv2.UnitTests.Helpers;
using Xunit;

namespace TwitterAPIv2.UnitTests.Application.Services
{
    public class TwitterServiceTest
    {
        private readonly TwitterService _twitterService;
        private readonly Mock<ITwitterAuthApi> _twitterAuthApi;
        private readonly Mock<HttpMessageHandler> _httpMessageHandler;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<TwitterService>> _mockLogger;
        private readonly IReadOnlyCollection<TweetStreamResponse> _tweets;

        public TwitterServiceTest()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();
            _mapper = AutoMapperHelper.Mapper;
            _mockLogger = new Mock<ILogger<TwitterService>>();
            _twitterAuthApi = new Mock<ITwitterAuthApi>();
            _tweets = new ReadOnlyCollection<TweetStreamResponse>(new List<TweetStreamResponse>()
            {
                new TweetStreamResponse()
                {
                    history_id = { }, id = "1602884697331093501", 
                    text = "tweet1", 
                    //entities = { }, 
                    public_metrics = { }
                },
                new TweetStreamResponse()
                {
                    history_id = { }, id = "1602884697331093502", 
                    text = "tweet2", 
                    //entities = { }, 
                    public_metrics = { }
                },
            });


            var twitterAuthSettings = new TwitterAuthSettings
            {
                BaseUrl = "https://fake-payment-url.com",
                ClientId = "id",
                ClientSecret = "secret",
                BearerToken = "ABCD1234"
            };

            IOptions<TwitterAuthSettings> settings =
                new OptionsWrapper<TwitterAuthSettings>(twitterAuthSettings);

            _twitterAuthApi
                .Setup(a => a.FetchToken(It.IsAny<string>(), It.IsAny<string>())).Returns("some_token");

            var httpClient = new HttpClient(_httpMessageHandler.Object);
            _twitterService = new TwitterService(httpClient, settings, _twitterAuthApi.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ShouldGetTweetsWithValidToken()
        {
            var expectedPayload = _tweets;
            var content = JsonConvert.SerializeObject(expectedPayload);

            _httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content)
                });

            var response = await _twitterService.GetSampleTweets();

            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Value);
        }

        [Fact]
        public async Task ShouldGetErrorResponseWithInvalidToken()
        {
            var expectedPayload = _tweets;
            var content = JsonConvert.SerializeObject(expectedPayload);

            _httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "GetStreamAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent(content)
                });

            var response = await _twitterService.GetSampleTweets();

            Assert.True(response.IsFailure);
            Assert.Equal("Sample tweet stream api error", response.Error.Code);
        }
    }
}
