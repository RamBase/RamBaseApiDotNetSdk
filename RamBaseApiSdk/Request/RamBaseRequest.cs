using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RamBaseApiSdk.Authentication;

namespace RamBase.Api.Sdk.Request
{   /// <summary>
    /// Class for sending requests
    /// </summary>
    internal class RamBaseRequest
    {
        private readonly HttpClient _httpClient;

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
        /// <param name="headers">Request headers</param>
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

            if (!string.IsNullOrWhiteSpace(parameters))
            {
                if (!parameters.StartsWith("?") && !uri.Contains('?'))
                    parameters = parameters.Insert(0, "?");

                else if (!parameters.StartsWith("?") && uri.Contains('?'))
                    parameters = parameters.Insert(0, "&");

                uri = string.Format("{0}{1}", uri, parameters);
            }

            HttpMethod httpMethod;

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, "Must be a valid http verb");
            }

            var request = new HttpRequestMessage(httpMethod, uri);

            if ((method == ApiResourceVerb.POST || method == ApiResourceVerb.PUT) && !string.IsNullOrEmpty(data))
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");

            foreach (var header in headers.Build())
                request.Headers.Add(header.Key, header.Value);

            return await _httpClient.SendAsync(request);
        }

        /// <summary>
        /// HTTP response validation. Converts response content to RamBaseError
        /// </summary>
        /// <param name="response">HTTP response for validation</param>
        /// <exception cref="RequestException">HTTP status code not successful</exception>
        public static async Task ValidateResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException();

            var json = await response.Content.ReadAsStringAsync();
            var responseError = JsonConvert.DeserializeObject<Error>(json);
            throw new RequestException(response, responseError);
        }

        /// <summary>
        /// Perform an async request for multiple resources in one HTTP request
        /// </summary>
        /// <param name="resources">List of relative path to a RamBase endpoint with parameters</param>
        /// <param name="headers">Request headers</param>
        /// <returns>Task with a list of the requested resources, sorted in the order requested</returns>
        public async Task<List<Resource>> GetBatchAsync(List<string> resources, Headers headers = null)
        {
            var url = "batch?";
            for (var i = 0; i < resources.Count; i++)
            {
                var resource = resources[i];
                url += $"resource{i}={resource}&";
            }
            url = url.TrimEnd('&');

            var response = await PerformRequestAsync(ApiResourceVerb.GET, url, headers: headers);
            var batch = JsonConvert.DeserializeObject<BatchResponse>(response.Content);
            foreach (var resource in batch.Resources)
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
