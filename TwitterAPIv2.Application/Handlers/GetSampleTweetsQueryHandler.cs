using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwitterAPIv2.Application.Dtos;
using TwitterAPIv2.Application.Queries;
using TwitterAPIv2.Core.Errors;
using TwitterAPIv2.Infrastructure.Services;

namespace TwitterAPIv2.Application.Handlers
{
    public class GetSampleTweetsQueryHandler : HandlerBase<IReadOnlyCollection<TweetDto>>, 
        IRequestHandler<GetSampleTweetsQuery, Result<IReadOnlyCollection<TweetDto>, Error>>
    {
        private readonly ILogger<GetSampleTweetsQueryHandler> _logger;
        private readonly ITwitterService _twitterService;
        private readonly IMapper _mapper;

        public GetSampleTweetsQueryHandler(ITwitterService twitterService, IMapper mapper, ILogger<GetSampleTweetsQueryHandler> logger)
        {
            _twitterService = twitterService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyCollection<TweetDto>, Error>> Handle(GetSampleTweetsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var tweetResponse = await  _twitterService.GetSampleTweets();
                var mappedTweets = _mapper.Map<IReadOnlyCollection<TweetDto>>(tweetResponse);

                return ToResult(mappedTweets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during {nameof(GetSampleTweetsQueryHandler)}. {ex.Message}");
                return ToResult(Errors.General.ApplicationError(ex.Message));
            }
        }
    }
}
