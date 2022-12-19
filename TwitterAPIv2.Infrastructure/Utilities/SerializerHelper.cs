using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TwitterAPIv2.Infrastructure.Utilities
{
    public static class SerializerHelper
    {
        public static T DeserializeFromSnakeCase<T>(string json) where T : class
        {
            var defaultJsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            return JsonConvert.DeserializeObject<T>(json, defaultJsonSettings);
        }
    }
}
