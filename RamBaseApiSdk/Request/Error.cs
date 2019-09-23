using System.Collections.Generic;
using System.Net;

namespace RamBase.Api.Sdk.Request
{
    public class Error
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string HttpStatusDescription { get; set; }
        public string Message { get; set; }
        public string TranslatedMessage { get; set; }
        public ErrorMessage ErrorMessage { get; set; }
        public List<ErrorMessageParameters> Parameters { get; set; }
        public string StackTrace { get; set; }
        public List<Error> InnerErrors { get; set; }
    }
}
