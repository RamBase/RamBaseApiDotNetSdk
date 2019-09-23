namespace RamBase.Api.Sdk.Meta
{
    using System;
    using System.Collections.Generic;

    public abstract class Field
    {
        public string Name { get; set; }
        public string Datatype { get; set; }
        public string XPath { get; set; }
        public string AccessDenied { get; set; }
        public int? ErrorMessageId { get; set; }
        public int? MissingPermissionId { get; set; }
        public int? AccessRuleId { get; set; }

        public FieldType FieldType { get; protected set; }

        internal Field(KeyValuePair<string, dynamic> field)
        {
            Name = field.Key;
            Datatype = field.Value.datatype;
            XPath = field.Value.xPath;
            AccessDenied = ((IDictionary<String, object>)field.Value).ContainsKey("accessDenied") ? field.Value.accessDenied : null;
            ErrorMessageId = ((IDictionary<String, object>)field.Value).ContainsKey("errorMessageId") ? field.Value.errorMessageId : null;
            MissingPermissionId = ((IDictionary<String, object>)field.Value).ContainsKey("missingPermissionId") ? field.Value.missingPermissionId : null;
            AccessRuleId = ((IDictionary<String, object>)field.Value).ContainsKey("accessRuleId") ? field.Value.accessRuleId : null;
        }
    }
}
