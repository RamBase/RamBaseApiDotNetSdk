using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http;

namespace RamBase.Api.Sdk.Request
{
    public class ApiResponse
    {
        public string Content { get; }
        public HttpResponseMessage Response { get; }

        public ApiResponse(string content, HttpResponseMessage response)
        {
            Content = content;
            Response = response;
        }

        /// <summary>
        /// Converts Content to ExpandoObject using Newtonsoft JSON
        /// </summary>
        /// <returns></returns>
        public dynamic ToDynamic()
        {
            if (string.IsNullOrEmpty(Content))
                return default;

            return JsonConvert.DeserializeObject<ExpandoObject>(Content);
        }

        /// <summary>
        /// Converts Content to T using Newtonsoft JSON with standard Newtonsoft settings
        /// </summary>
        /// <typeparam name="T">Class to convert to</typeparam>
        /// <returns>Content converted to T</returns>
        public T To<T>()
        {
            return JsonConvert.DeserializeObject<T>(Content);
        }

        /// <summary>
        /// Converts Content to T using Newtonsoft JSON
        /// </summary>
        /// <typeparam name="T">Class to convert to</typeparam>
        /// <param name="deserializerSettings">Settings for Newtonsoft JSON deserializer</param>
        /// <returns>Content converted to T</returns>
        public T To<T>(JsonSerializerSettings deserializerSettings)
        {
            return JsonConvert.DeserializeObject<T>(Content, deserializerSettings);
        }
    }
}
