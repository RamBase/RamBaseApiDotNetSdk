using Newtonsoft.Json;

namespace RamBase.Api.Sdk.Authentication
{
    public class Target
    {
        [JsonProperty("target_name")]
        public string TargetName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
