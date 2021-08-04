using System.Collections.Generic;

namespace RamBase.Api.Sdk.Request
{
    public class Headers
    {
        public string ImpersonateUser { get; set; }

        public Dictionary<string, string> RequestHeaders { get; set; } = new Dictionary<string, string>();

        public void Add(string name, string value)
        {
            RequestHeaders.Add(name, value);
        }

        public Dictionary<string, string> Build()
        {
            if (!string.IsNullOrEmpty(ImpersonateUser))
                if (RequestHeaders.ContainsKey("Impersonate-User"))
                    RequestHeaders["Impersonate-User"] = ImpersonateUser;
                else
                    RequestHeaders.Add("Impersonate-User", ImpersonateUser);
            return RequestHeaders;
        }
    }
}