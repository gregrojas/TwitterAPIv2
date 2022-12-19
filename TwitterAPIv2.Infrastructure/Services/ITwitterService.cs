using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterAPIv2.Core.Errors;
using TwitterAPIv2.Infrastructure.Responses;

namespace TwitterAPIv2.Infrastructure.Services
{
    public interface ITwitterService
    {
        Task<Result<IReadOnlyCollection<TweetStreamResponse>, Error>> GetSampleTweets();
    }
}
