// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.JsonSchema
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class JsonSchema : IJsonSchemaBase
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "$schema")]
        public string Schema { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "required")]
        public IEnumerable<string> Required { get; set; }

        [DataMember(Name = "definitions")]
        // Key = resource name
        public Dictionary<string, JsonDefinition> Definitions { get; set; }

        [DataMember(Name = "properties")]
        public Dictionary<string, JsonProperty> Properties { get; set; }

        // TODO patternProperties

        private const string DefaultSchema = "http://json-schema.org/draft-04/schema#";
        public JsonSchema() : this(DefaultSchema) { }

        public JsonSchema(string schema)
        {
            Schema = schema;
        }
    }
}