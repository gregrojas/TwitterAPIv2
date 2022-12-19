using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TwitterAPIv2.Application.Dtos
{
    public class TweetDto
    {
        [JsonProperty(PropertyName = "edit_history_tweet_ids")]
        public List<string> history_id { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string text { get; set; }
        //[JsonProperty(PropertyName = "entities")]
        //public List<Entities> entities { get; set; }
        [JsonProperty(PropertyName = "public_metrics")]
        public Metrics public_metrics { get; set; }
    }
}
