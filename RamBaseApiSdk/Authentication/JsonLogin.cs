namespace RamBase.Api.Sdk.Authentication
{
    internal class JsonLogin
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string expires_in { get; set; }
        public bool is_target_test_system { get; set; }
    }
}
