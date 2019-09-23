using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Request
{
    //source: https://stackoverflow.com/questions/29980580/deserialize-json-object-property-to-string
    class JsonConverterObjectToString : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(JTokenType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Object)
            {
                return token.ToString();
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var token = JToken.Parse(value.ToString());
            writer.WriteToken(token.CreateReader());
        }
    }
}
