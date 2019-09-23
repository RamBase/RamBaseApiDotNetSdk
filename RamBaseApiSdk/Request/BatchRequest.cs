using System;
using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Request
{
    public class BatchRequest
    {
        Dictionary<string, string> Requests = new Dictionary<string, string>();

        public void AddRequest(string uri, GetParameters parameters)
        {
            Requests.Add(uri, parameters.Build());
        }

        public void AddRequest(string uri, string parameters)
        {
            Requests.Add(uri, parameters);
        }

        public List<string> Build()
        {
            List<string> requests = new List<string>();
            foreach (KeyValuePair<string, string> request in Requests)
                requests.Add(request.Key + request.Value);
            return requests;
        }
    }
}
