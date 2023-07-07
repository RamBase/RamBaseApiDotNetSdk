using RamBase.Api.Sdk.Request;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RamBase.Api.Sdk.Operations
{
    internal class RamBaseOperations
    {
        private readonly RamBaseRequest _request;

        public RamBaseOperations(RamBaseRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// Asynchronously start given operation for the given resource
        /// </summary>
        /// <param name="operationId">Operation id</param>
        /// <param name="resourceUri">Relative path to resource</param>
        /// <param name="data">Http body as JSON</param>
        /// <param name="parameters">Url query parameters</param>
        /// <param name="headers">Request headers</param>
        /// <returns>Task with OperationInstance</returns>
        /// <exception cref="RequestException">When HTTP status is not successful</exception>
        public async Task<OperationInstance> StartOperationAsync(int operationId, string resourceUri, string data = "", string parameters = "", Headers headers = null)
        {
            var uri = string.Format("{0}/api-operations/{1}/instances", resourceUri, operationId);
            var response = await _request.SendRequestAsync(ApiResourceVerb.POST, uri, data, parameters, headers);

            if (!response.IsSuccessStatusCode)
                throw new RequestException(response);

            var json = await response.Content.ReadAsStringAsync();
            var operation = JsonConvert.DeserializeObject<OperationWrapper>(json).OperationInstance;

            return operation;
        }

        /// <summary>
        /// Asynchronously get operation status
        /// </summary>
        /// <param name="operationInstance">Operation</param>
        /// <returns>Task with new OperationInstance</returns>
        public async Task<OperationInstance> GetOperationInstanceStatusAsync(OperationInstance operationInstance, Headers headers = null)
        {
            var response = await _request.PerformRequestAsync(ApiResourceVerb.GET, operationInstance.OperationInstanceLink, headers:headers);
            return JsonConvert.DeserializeObject<OperationWrapper>(response.Content).OperationInstance;
        }
    }
}
