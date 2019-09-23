using System.Collections.Generic;

namespace RamBase.Api.Sdk.Meta
{
    public class SortableField
    {
        public string Name { get; set; }
        public string Datatype { get; set; }
        public string TranslatedName { get; set; }

        public SortableField(KeyValuePair<string, dynamic> sortableField)
        {
            Name = sortableField.Key;
            Datatype = sortableField.Value.datatype;
            TranslatedName = sortableField.Value.translatedName;
        }
    }
}