using System.Collections.Generic;

namespace RamBase.Api.Sdk.DomainValues
{
    public class DomainValueWrapper
    {
        public List<DomainValue> DomainValues { get; set; }
        public Paging Paging { get; set; }
    }
}
