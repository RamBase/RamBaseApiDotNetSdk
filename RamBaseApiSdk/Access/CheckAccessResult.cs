namespace RamBase.Api.Sdk.Access
{
    public class CheckAccessResult
    {
        public string TranslatedErrorMessage { get; set; }
        public RamBaseObject Object { get; set; }
        public RamBasePermission MissingPermission { get; set; }
    }
}

