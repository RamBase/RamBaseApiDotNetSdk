using System.Collections.Generic;

namespace RamBase.Api.Sdk.Access
{
    public class CheckAccessRequest
    {
        public List<CheckAccessRequest> Requests { get; set; }
        
        public void AddAccessRule(int accessRule, string doc = "")
        {
            Requests.Add(new CheckAccessRequestWithAccessRuleId(accessRule, doc));
        }

        public void AddOperation(int operationId, string uri)
        {
            Requests.Add(new CheckAccessRequestWithOperationId(operationId, uri));
        }
    }
}
