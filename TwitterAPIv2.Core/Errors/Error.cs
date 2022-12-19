using System.Collections.Generic;
using TwitterAPIv2.Core.Errors;
using Newtonsoft.Json;

namespace TwitterAPIv2.Core.Errors
{
    public class Error : ValueObject
    {
        public string Code { get; }
        public string Message { get; }
        public IEnumerable<object> Details { get; }

        [JsonConstructor]
        internal Error(string code, string message, IEnumerable<object> details = null)
        {
            Code = code;
            Message = message;
            Details = details;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Only the Code property is considered the equality value for
            // an Error. The Message property can vary across usages.
            yield return Code;
        }
    }
}
