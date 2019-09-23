using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace RamBase.Api.Sdk.Request
{
    public class RequestException : Exception
    {
        public Error Error { get; }
        public HttpResponseMessage Response { get; }

        public RequestException(HttpResponseMessage response) : base(response.ReasonPhrase)
        {
            Response = response;
            Error = JsonConvert.DeserializeObject<Error>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public RequestException(HttpResponseMessage response, Error responseError) : base(responseError.TranslatedMessage)
        {
            Response = response;
            Error = responseError;
        }
    }
}
