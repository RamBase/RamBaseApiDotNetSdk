using RamBase.Api.Sdk.Request;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace RamBase.Api.Sdk.Authentication
{
    public class LoginException : Exception
    {
        public Error Error { get; }
        public HttpResponseMessage Response { get; }
        public LoginException(HttpResponseMessage response) : base(response.ReasonPhrase)
        {
            Response = response;
            Error = JsonConvert.DeserializeObject<Error>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public LoginException(string message) : base(message)
        {
            
        }
    }
}
