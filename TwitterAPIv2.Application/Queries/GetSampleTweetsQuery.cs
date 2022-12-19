using CSharpFunctionalExtensions;
using MediatR;
using System.Collections.Generic;
using TwitterAPIv2.Application.Dtos;
using TwitterAPIv2.Core.Errors;

namespace TwitterAPIv2.Application.Queries
{
    public class GetSampleTweetsQuery : IRequest<Result<IReadOnlyCollection<TweetDto>, Error>>
    {
        public GetSampleTweetsQuery()
        {
        }
    }
}
