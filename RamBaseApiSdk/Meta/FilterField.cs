using System;
using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Meta
{
    public class FilterField
    {
        public string Name { get; set; }
        public string TranslatedName { get; set; }
        public string Description { get; set; }
        public string Datatype { get; set; }
        public string ApplicableDomainValues { get; set; }
        public string ValidValuesDov { get; set; }
        public List<Macro> AvailableMacros { get; set; }

        public FilterField(KeyValuePair<string, dynamic> filterField)
        {
            Name = filterField.Key;
            TranslatedName = filterField.Value.translatedName;
            Description = filterField.Value.description;
            Datatype = filterField.Value.datatype;
            ApplicableDomainValues = filterField.Value.applicableDomainValues;
            ValidValuesDov = filterField.Value.validValuesDov;
            AvailableMacros = GetMacros(filterField.Value.availableMacros);
        }
        private List<Macro> GetMacros(dynamic macrosDynamic)
        {
            List<Macro> macros = new List<Macro>();
            foreach (KeyValuePair<string, dynamic> macro in macrosDynamic)
                macros.Add(new Macro()
                {
                    Name = macro.Key,
                    AllowedUnits = ((IDictionary<String, object>)macro.Value).ContainsKey("allowedUnits") ? macro.Value.allowedUnits : null,
                    DefaultUnit = ((IDictionary<String, object>)macro.Value).ContainsKey("defaultUnit") ? macro.Value.defaultUnit : null
                });

            return macros;
        }
    }
}
