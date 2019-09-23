namespace RamBase.Api.Sdk.DomainValues
{
    public class Paging
    {
        public int Position { get; set; }
        public string FirstPage { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }
        public string LastPage { get; set; }
    }
}
