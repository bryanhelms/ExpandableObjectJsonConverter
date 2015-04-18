using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ExpandableObjectJsonConverter
{
    public class ExpandableObjectJsonConverter<TExpandable> : JsonConverter
        where TExpandable : ExpandableObjectBase, new()
    {
        public override bool CanConvert(Type objectType)
        {
            // Expandable objects should be assignable to string (ID only) or the object we're 
            // trying to convert to (the expanded version).
            return typeof(TExpandable).IsAssignableFrom(objectType) || typeof(string).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) {
                // No value was provided at all, so we return null
                return null;
            } else if (reader.TokenType == JsonToken.String) {
                // This isn't the expanded object. Still make the intended object, but only set the ID
                var expanded = new TExpandable();
                expanded.Id = (string) reader.Value;
                expanded.IsExpanded = false;

                return expanded;
            } else if (reader.TokenType == JsonToken.StartObject) {
                // We're dealing with an actual JSON object here, so try to deserialize to 
                // the specified type. As a courtesy, we mark expanded objects as such so 
                // the caller can easily know what happened here
                var jObj = JObject.Load(reader);
                var expanded = jObj.ToObject<TExpandable>(serializer);
                expanded.IsExpanded = true;

                return expanded;
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead { get { return true; } }
        // Expanded objects really only happen in responses, so no reason to serialize this
        public override bool CanWrite { get { return false; } }
    }
}