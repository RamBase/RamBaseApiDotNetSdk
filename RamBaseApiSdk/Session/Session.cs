namespace RamBase.Api.Sdk.Sessions
{
    public class Session
    {
        public int? Pid { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? ContactId { get; set; }
        public string Database { get; set; }
        public string RamBaseName { get; set; }
        public string ApiClientId { get; set; }
    }
}