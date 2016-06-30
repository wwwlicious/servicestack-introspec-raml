// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.JsonSchema
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Text;

    [DataContract]
    public class JsonSchemaTypeDefinition
    {
        /*[DataMember(Name = "id")]
        public string Id { get; set; } // Is this of use???

        [DataMember(Name = "title")]
        public string Title { get; set; } // Is this of use???*/

        [DataMember(Name = "$schema")]
        public string Schema { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "required")]
        public IEnumerable<string> Required { get; set; }

        [DataMember(Name = "properties")]
        public Dictionary<string, JsonSchemaProperty> Properties { get; set; }

        [DataMember(Name = "definitions")]
        public IEnumerable<JsonSchemaDefinition> Definitions { get; set; }

        //patternProperties
        //additionalProperties <- always false??

        private const string DefaultSchema = "http://json-schema.org/draft-04/schema#";
        public JsonSchemaTypeDefinition() : this(DefaultSchema) { }

        public JsonSchemaTypeDefinition(string schema)
        {
            Schema = schema;
        }

        public static void DoStuff()
        {
            using (var config = JsConfig.BeginScope())
            {
                config.EmitCamelCaseNames = true;

                var schema = new JsonSchemaTypeDefinition();
                schema.Type = "object";
                schema.Required = new[] { "Hi" };
                schema.Properties = new Dictionary<string, JsonSchemaProperty>();
                schema.Properties.Add("Hi",
                    new JsonSchemaProperty
                    {
                        Type = new List<string> { "string", "null" }
                    });

                var defs = new List<JsonSchemaDefinition>();
                var anotherClass = new JsonSchemaDefinition
                {
                    Type = new[] { "object", "null" },
                    Properties = new Dictionary<string, JsonSchemaProperty>
                    {
                        { "foo", new JsonSchemaProperty { Type = new[] { "integer" } } }
                    }
                };
                defs.Add(anotherClass);
                schema.Definitions = defs;

                var x = schema.ToJson();
            }
        }
    }

    public class JsonSchemaDefinition
    {
        public IEnumerable<string> Type { get; set; }
        public IEnumerable<string> Required { get; set; }

        public Dictionary<string, JsonSchemaProperty> Properties { get; set; }

        // OneOf
        // Pattern
    }

    public class JsonSchemaProperty
    {
        public IEnumerable<string> Type { get; set; }

        public JsonSchemaRef Items { get; set; } // this has $ref inside
    }

    [DataContract]
    public class JsonSchemaRef
    {
        [DataMember(Name = "$ref")]
        public string Ref { get; private set; }
    }
}

// Type can be enum
// Type for definition is []