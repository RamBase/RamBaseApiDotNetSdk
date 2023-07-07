using RamBase.Api.Sdk.Request;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RamBase.Api.Sdk.DomainValues
{
    internal class RamBaseDomainValues
    {
        private readonly RamBaseRequest _request;
        public RamBaseDomainValues(RamBaseRequest ramBaseRequest)
        {
            _request = ramBaseRequest;
        }

        /// <summary>
        /// Asynchronously get applicable domain values for given object and field
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="field">Field</param>
        /// <param name="parameters">Url query parameters</param>
        /// <param name="headers">Request headers</param>
        /// <returns>Task with list of DomainValues</returns>
        public async Task<List<DomainValue>> GetApplicableDomainValuesAsync(string obj, string field, string parameters, Headers headers = null)
        {
            if (string.IsNullOrEmpty(parameters))
                parameters = "?";
            else
                parameters += "&";

            parameters += $"object={obj}&field={field}";
            parameters += "&$top=$max";
            const string uri = "system/domain-values/applicable-values";
            var response = await _request.PerformRequestAsync(ApiResourceVerb.GET, uri, parameters: parameters, headers: headers);
            var res = JsonConvert.DeserializeObject<DomainValueWrapper>(response.Content);
            return res.DomainValues;
        }
    }
}
