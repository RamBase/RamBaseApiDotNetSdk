using System.Collections.Generic;
using System.Linq;

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
            return Requests.Select(request => request.Key + request.Value).ToList();
        }
    }
}
