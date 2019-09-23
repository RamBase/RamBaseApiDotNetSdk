using System.Collections.Generic;

namespace RamBase.Api.Sdk.Meta
{
    class FieldPut : FieldPost
    {
        public FieldPut(KeyValuePair<string, dynamic> field) : base(field)
        {
            FieldType = FieldType.PUT;
        }
    }
}
