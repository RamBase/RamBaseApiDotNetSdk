using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using RamBaseApiSdk.Authentication;

namespace RamBase.Api.Sdk.Request
{   /// <summary>
    /// Class for sending requests
    /// </summary>
    internal class RamBaseRequest
    {
        private HttpClient _httpClient;

        public RamBaseRequest(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Async HTTP Request with validation
        /// </summary>
        /// <param name="method">HTTP Method</param>
        /// <param name="uri">Relative or explicit URI</param>
        /// <param name="data">HTTP Body as JSON</param>
        /// <param name="parameters">URL Request parameters</param>
        /// <param name="Headers">Request headers</param>
        /// <returns>ApiResponse containing HTTP request body as JSON</returns>
        public async Task<ApiResponse> PerformRequestAsync(ApiResourceVerb method, string uri, string data = "", string parameters = "", Headers headers = null)
        {
            var response = await SendRequestAsync(method, uri, data, parameters, headers);
            await ValidateResponse(response);
            return new ApiResponse(await response.Content.ReadAsStringAsync(), response);
        }

        /// <summary>
        /// Basic async HTTP Request method. Does not validate response
        /// </summary>
        /// <param name="method">HTTP Method</param>
        /// <param name="uri">Relative or explicit URI</param>
        /// <param name="data">HTTP Body as JSON</param>
        /// <param name="parameters">URL Request parameters</param>
        /// <param name="headers">Request headers</param>
        /// <returns>Task with HTTP response</returns>
        public async Task<HttpResponseMessage> SendRequestAsync(ApiResourceVerb method, string uri, string data, string parameters, Headers headers)
        {
            headers = headers ?? new Headers();

            if (!string.IsNullOrWhiteSpace(parameters) && !parameters.StartsWith("?") && !string.IsNullOrWhiteSpace(uri) && !uri.Contains('?'))
                parameters = "?" + parameters;

            uri = string.Format("{0}{1}", uri, parameters);

            HttpMethod httpMethod = HttpMethod.Get;

            switch (method)
            {
                case ApiResourceVerb.GET:
                    httpMethod = HttpMethod.Get;
                    break;
                case ApiResourceVerb.DELETE:
                    httpMethod = HttpMethod.Delete;
                    break;
                case ApiResourceVerb.PUT:
                    httpMethod = HttpMethod.Put;
                    break;
                case ApiResourceVerb.POST:
                    httpMethod = HttpMethod.Post;
                    break;
            }

            var request = new HttpRequestMessage(httpMethod, uri);

            if ((method == ApiResourceVerb.POST || method == ApiResourceVerb.PUT) && !string.IsNullOrEmpty(data))
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");

            foreach (KeyValuePair<string, string> header in headers.RequestHeaders)
                request.Headers.Add(header.Key, header.Value);

            return await _httpClient.SendAsync(request);
        }

        /// <summary>
        /// HTTP response validation. Converts response content to RamBaseError
        /// </summary>
        /// <param name="response">HTTP response for validation</param>
        /// <exception cref="RequestException">HTTP status code not successful</exception>
        public async Task ValidateResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();

            string json = await response.Content.ReadAsStringAsync();
            Error responseError = JsonConvert.DeserializeObject<Error>(json);
            throw new RequestException(response, responseError);
        }

        /// <summary>
        /// Perform an async request for multiple resources in one HTTP request
        /// </summary>
        /// <param name="resources">List of relative path to a RamBase endpoint with parameters</param>
        /// <param name="Headers">Request headers</param>
        /// <returns>Task with a list of the requested resources, sorted in the order requested</returns>
        public async Task<List<Resource>> GetBatchAsync(List<string> resources, Headers headers = null)
        {
            string url = "batch?";
            for (int i = 0; i < resources.Count; i++)
            {
                string resource = resources[i];
                url += $"resource{i}={resource}&";
            }
            url = url.TrimEnd('&');

            ApiResponse response = await PerformRequestAsync(ApiResourceVerb.GET, url, headers:headers);
            BatchResponse batch = JsonConvert.DeserializeObject<BatchResponse>(response.Content);
            foreach (Resource resource in batch.Resources)
            {
                if (resource.Response.Substring(0, 30).TrimStart('{').Trim().ToLower().StartsWith("\"httpstatuscode\""))
                    resource.Error = JsonConvert.DeserializeObject<Error>(resource.Response);
                else
                    resource.ApiResponse = new ApiResponse(resource.Response, null);
            }
            return batch.Resources.OrderBy(r => r.BatchId).ToList();
        }
    }
}
