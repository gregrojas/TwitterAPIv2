using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterAPIv2.Infrastructure.Responses
{
    public class Annotations
    {
        public int start { get; set; }
        public int end { get; set; }
        public decimal probability { get; set; }
        public string type { get; set; }
        public string normalized_text { get; set; }
    }
}
