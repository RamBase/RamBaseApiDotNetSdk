using RamBase.Api.Sdk.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RamBase.Api.Sdk.Access
{
    internal class RamBaseAccess
    {
        private RamBaseRequest _request;
        public RamBaseAccess(RamBaseRequest ramBaseRequest)
        {
            _request = ramBaseRequest;
        }
        /// <summary>
        /// Asynchronously checks access for given access check requests
        /// </summary>
        /// <param name="accessCheckRequests">List of accesses to check</param>
        /// <param name="parameters">Url query parameters</param>
        /// <param name="Headers">Request headers</param>
        /// <returns>Task with list of CheckAccesses</returns>
        public async Task<List<CheckAccessResult>> CheckAccessAsync(List<CheckAccessRequest> accessCheckRequests, string parameters = "", Headers headers = null)
        {
            string url = "system/api/api-operations/check-access";

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string data = JsonConvert.SerializeObject(
                new
                {
                    apiOperations = accessCheckRequests
                },
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = contractResolver
                }
             );

            var response = await _request.PerformRequestAsync(ApiResourceVerb.POST, url, data, parameters, headers);
            var res = JsonConvert.DeserializeObject<CheckAccessWrapper>(response.Content);
            return res.CheckAccessResults;
        }
    }
}
