namespace TwitterAPIv2.Infrastructure.Responses
{
    public class OAuthTokenResponse
    {
        public string AccessToken { get; }
        public int ExpiresIn { get; }

        public OAuthTokenResponse(string accessToken, int expiresIn)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
        }
    }
}