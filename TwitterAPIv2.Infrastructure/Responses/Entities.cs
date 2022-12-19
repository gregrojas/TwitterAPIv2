using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace TwitterAPIv2.Infrastructure.Responses
{
    public class Entities
    {
        [JsonProperty(PropertyName = "annotations")]
        public List<Annotations> annotations { get; set; }
        [JsonProperty(PropertyName = "hashtags")]
        public List<Hashtags> hashtags { get; set; }
    }
}
