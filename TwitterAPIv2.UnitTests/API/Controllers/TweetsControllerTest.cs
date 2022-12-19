using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using TwitterAPIv2.API.Controllers;
using TwitterAPIv2.Application.Dtos;
using TwitterAPIv2.Application.Queries;
using TwitterAPIv2.Core.Errors;
using Xunit;

namespace TwitterAPIv2.UnitTests.API.Controllers
{
    public class TweetsControllerTest
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly IReadOnlyCollection<TweetDto> _tweets;

        public TweetsControllerTest()
        {
            _mockMediator = new Mock<IMediator>();
            _tweets = new ReadOnlyCollection<TweetDto>(new List<TweetDto>()
            {
                new TweetDto()
                {
                    history_id = { }, 
                    id = "1602884697331093501", 
                    text = "tweet1", 
                    //entities = { }, 
                    public_metrics = { }
                },
                new TweetDto()
                {
                    history_id = { }, 
                    id = "1602884697331093502", 
                    text = "tweet2", 
                    //entities = { }, 
                    public_metrics = { }
                },
            });
        }

        [Fact]
        public async void CanGetVolumeStream()
        {
            _mockMediator.Setup(p => p.Send(It.IsAny<GetSampleTweetsQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Result.Success<IReadOnlyCollection<TweetDto>, Error>(_tweets)));

            var controller = new TweetsController(_mockMediator.Object);

            //Act
            var result = await controller.GetSampleStream();

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var tweets = okObjectResult.Value as IReadOnlyCollection<TweetDto>;
            Assert.NotNull(tweets);
            Assert.Equal(2, tweets.Count);
        }
    }
}
