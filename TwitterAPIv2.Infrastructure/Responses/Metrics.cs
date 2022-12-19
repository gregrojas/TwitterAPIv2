using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterAPIv2.Infrastructure.Responses
{
    public class Metrics
    {
        public int retweet_count { get; set; }
        public int reply_count { get; set; }
        public int like_count { get; set; }
        public int quote_count { get; set; }
    }
}
