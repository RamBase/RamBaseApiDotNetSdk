using System;
using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Access
{
    internal class CheckAccessRequestWithOperationId : CheckAccessRequest
    {
        public int ApiOperationId { get; set; }
        public string Uri { get; set; }

        public CheckAccessRequestWithOperationId(int apiOperationId, string uri)
        {
            ApiOperationId = apiOperationId;
            Uri = uri;
        }
    }
}
