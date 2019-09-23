using System;
using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Access
{
    internal class CheckAccessRequestWithAccessRuleId : CheckAccessRequest
    {
        public int AccessRule { get; set; }
        public string Doc { get; set; }

        public CheckAccessRequestWithAccessRuleId(int accessRule, string doc = null)
        {
            AccessRule = accessRule;
            Doc = doc;
        }
    }
}
