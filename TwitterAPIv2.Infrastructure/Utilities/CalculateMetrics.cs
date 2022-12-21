using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterAPIv2.Infrastructure.Utilities
{
    public class CalculateMetrics : ICalculateMetrics
    {
        public CalculateMetrics()
        { }

        public void TopHashTags(List<string> tags, Dictionary<string, int> topHashTags)
        {
            foreach (string tag in tags)
            {
                int count = 0;
                if (topHashTags.ContainsKey(tag))
                {
                    count = topHashTags[tag];
                }
                topHashTags[tag] = count + 1;
            }

            var items = from pair in topHashTags
                        orderby pair.Value ascending
                        select pair;

            //Count tag number of occurrences
            if (tags.Count <= 10)
            {
                foreach (KeyValuePair<string, int> pair in items)
                {
                    Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                }
            }
        }
    }
}
