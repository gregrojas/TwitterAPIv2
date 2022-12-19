namespace TwitterAPIv2.Infrastructure.Services
{
    public interface ITwitterAuthApi
    {
        string FetchToken(string clientId, string clientSecret);
    }
}
