using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace RamBase.Api.Sdk.Meta
{
    internal class MetadataResponse
    {
        public Admittance Admittance { get; set; }
        public ExpandoObject Fields { get; set; }
        public ExpandoObject FilterFields { get; set; }
        public ExpandoObject SortableFields { get; set; }
        public List<NamedFilter> NamedFilters { get; set; }
    }
}
