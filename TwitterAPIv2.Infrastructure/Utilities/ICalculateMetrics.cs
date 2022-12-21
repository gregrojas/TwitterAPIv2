using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterAPIv2.Infrastructure.Utilities
{
    public interface ICalculateMetrics
    {
        void TopHashTags(List<string> tags, Dictionary<string, int> topHashTags);
    }
}
