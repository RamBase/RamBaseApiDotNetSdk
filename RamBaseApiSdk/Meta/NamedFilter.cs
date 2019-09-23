using System.Collections.Generic;

namespace RamBase.Api.Sdk.Meta
{
    public class NamedFilter
    {
        public string FilterName { get; set; }
        public string FriendlyFilterName { get; set; }
        public string FilterString { get; set; }
        public List<string> UncombinableWith { get; set; }
    }
}
