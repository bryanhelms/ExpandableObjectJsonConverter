using Newtonsoft.Json;
using System;

namespace ExpandableObjectJsonConverter
{
    public class TopLevelObject
    {
        public string FirstValue { get; set; }
        [JsonConverter(typeof(ExpandableObjectJsonConverter<ExampleExpandableObject>))]
        public ExampleExpandableObject Example { get; set; }
    }
}