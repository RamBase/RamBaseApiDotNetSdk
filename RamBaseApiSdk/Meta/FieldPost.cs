using System.Collections.Generic;

namespace RamBase.Api.Sdk.Meta
{
    class FieldPost : Field
    {
        public string TranslatedName { get; set; }
        public string Description { get; set; }
        public string ApplicableDomainValues { get; set; }
        public string RegularExpression { get; set; }
        public bool IsRequired { get; set; }
        public dynamic MinimumValue { get; set; }
        public dynamic MaximumValue { get; set; }
        public dynamic DefaultValue { get; set; }
        
        public FieldPost(KeyValuePair<string, dynamic> field) : base(field)
        {
            TranslatedName = field.Value.translatedName;
            Description = field.Value.description;
            ApplicableDomainValues = field.Value.applicableDomainValues;
            IsRequired = field.Value.isRequired;
            RegularExpression = field.Value.regularExpression;
            MinimumValue = field.Value.minimumValue;
            MaximumValue = field.Value.maximumValue;
            DefaultValue = field.Value.defaultValue;
            FieldType = FieldType.POST;
        }
    }
}
