using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwitterAPIv2.Infrastructure.Services;

namespace Common.Presentation
{
    public static class TwitterApiExtension
    {
        public static void AddTwitterApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TwitterAuthSettings>(configuration.GetSection(TwitterAuthSettings.Position));
            services.AddHttpClient<TwitterService, TwitterService>();
        }
    }
}
