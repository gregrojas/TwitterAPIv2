using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterAPIv2.Application.Dtos
{
    public class Entities
    {
        public IList<Annotations> annotations { get; set; }
        public IList<Hashtags> hashtags { get; set; }
    }
}
