using System;

namespace TwitterAPIv2.Core.Errors
{
    using static String;

    public static class Errors
    {
        public static class TweetManagement
        {
            public static Error StreamError(string message)
            {
                return new Error("Sample tweet stream api error", message);
            }
        }

        public static class General
        {
            public static Error ApplicationError(string message = null)
            {
                message = message != null ? $": {message}" : "";
                return new Error("general.error.processing.request", Format("An error occurred while processing the request {0}", message));
            }
        }
    }
}
