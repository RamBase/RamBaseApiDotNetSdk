using RamBase.Api.Sdk.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Meta
{
    public class MetadataParameters : QueryParameters
    {
        public string Expand { get; set; }

        public override string ToString()
        {
            string value = base.ToString();

            if (!value.EndsWith("?"))
                value += "&";

            if (!string.IsNullOrEmpty(Expand))
                value += $"$expand={Expand}&";

            return value.TrimEnd('&');
        }
    }
}
