using System.Collections.Generic;

namespace PostageApp.Abstractions
{
    public class GetSuppressionListResponseData
    {
        public IReadOnlyDictionary<string, GetSuppressionListResponseDataItem> Recipients { get; set; }
    }
}
