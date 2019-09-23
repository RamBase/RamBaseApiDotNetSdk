using RamBase.Api.Sdk.Request;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RamBase.Api.Sdk.Meta
{
    internal class RamBaseMetadata
    {
        private RamBaseRequest _request;

        public RamBaseMetadata(RamBaseRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// Asynchronously gets metadata for given resource
        /// </summary>
        /// <param name="uri">Relative or explicit path to a resource/endpoint</param>
        /// <param name="verb">Metadata for given verb</param>
        /// <param name="parameters">HTTP request parameters</param>
        /// <returns>Task with metadata</returns>
        /// <exception cref="RequestException">HTTP status not successful</exception>
        public async Task<Metadata> GetMetadataAsync(string uri, ApiResourceVerb verb, string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
                parameters = "?";
            else
                parameters += "&";

            parameters += $"uri={uri}&verb={verb}";
            parameters += "&$expand=Description";
            string url = "$metadata";
            ApiResponse response = await _request.PerformRequestAsync(ApiResourceVerb.GET, url, parameters: parameters);
            MetadataResponse metadataResponse = JsonConvert.DeserializeObject<MetadataResponse>(response.Content);
            return new Metadata(metadataResponse, verb);
        } 
    }
}
