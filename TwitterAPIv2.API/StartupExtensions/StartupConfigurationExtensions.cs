using System;
using Microsoft.Extensions.Configuration;

namespace TwitterAPIv2.API.StartupExtensions
{
    /// <summary>
    /// Extension methods for configuration
    /// </summary>
    /// <remarks>
    /// Collection of extension methods used to simplify IConfiguration interactions
    /// </remarks>
    public static class StartupConfigurationExtensions
    {
        /// <summary>
        /// Gets string value from configuration
        /// </summary>
        /// <remarks>
        /// Gets value or throws invalid operation exception if not found
        /// </remarks>
        public static string TryGetStringValue(this IConfiguration configuration, string configurationKey)
        {
            var configurationValue = configuration.GetValue<string>(configurationKey);
            if (configurationValue == null)
            {
                throw new InvalidOperationException($"No {configurationKey} config variable found");
            }
            return configurationValue;
        }
    }
}
