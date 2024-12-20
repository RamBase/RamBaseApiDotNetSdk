using Newtonsoft.Json;

namespace RamBase.Api.Sdk.Request
{
    public class Resource
    {
        public string Uri { get; set; }
        public string BatchId { get; set; }
        [JsonConverter(typeof(JsonConverterObjectToString))]
        public string Response { get; set; }
        public ApiResponse ApiResponse { get; set; }
        public Error Error { get; set; }
    }
}
