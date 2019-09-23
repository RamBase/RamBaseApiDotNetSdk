namespace RamBase.Api.Sdk.Meta
{
    using System.Collections.Generic;

    class FieldGet : Field
    {
        public string Description { get; set; }
        public string TranslatedName { get; set; }

        public FieldGet(KeyValuePair<string, dynamic> field) : base(field)
        {
            Description = field.Value.description;
            TranslatedName = field.Value.translatedName;
            FieldType = FieldType.GET;
        }
    }
}
