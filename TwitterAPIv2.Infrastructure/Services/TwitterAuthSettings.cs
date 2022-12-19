namespace TwitterAPIv2.Infrastructure.Services
{
    public class TwitterAuthSettings
    {
        public const string Position = "TwitterAuth";

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string BaseUrl { get; set; }
        public string AuthUrl { get; set; }
        public string BearerToken { get; set; }
    }
}
