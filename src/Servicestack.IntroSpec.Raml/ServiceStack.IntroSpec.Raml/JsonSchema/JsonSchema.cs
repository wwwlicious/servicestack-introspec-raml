// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.JsonSchema
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Services;
    using Text;

    [DataContract]
    public class JsonSchema : IJsonSchemaBase
    {
        /*[DataMember(Name = "id")]
        public string Id { get; set; } // Is this of use???*/

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
        public Dictionary<string, JsonSchemaDefinition> Definitions { get; set; }

        [DataMember(Name = "properties")]
        public Dictionary<string, JsonSchemaProperty> Properties { get; set; }

        //patternProperties
        //additionalProperties <- always false??

        private const string DefaultSchema = "http://json-schema.org/draft-04/schema#";
        public JsonSchema() : this(DefaultSchema) { }

        public JsonSchema(string schema)
        {
            Schema = schema;
        }
    }

    public class JsonSchemaDefinition : IJsonSchemaBase
    {
        public IEnumerable<string> Type { get; set; }
        public IEnumerable<string> Required { get; set; }

        public Dictionary<string, JsonSchemaProperty> Properties { get; set; }

        // OneOf
        // Pattern
    }

    public interface IJsonSchemaBase
    {
        IEnumerable<string> Required { get; set; }

        Dictionary<string, JsonSchemaProperty> Properties { get; set; }
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

    public static class TestRunner
    {
        public static void DoStuff()
        {
            var documentationProvider = new ApiDocumentationProvider();
            var documentation = documentationProvider.GetApiDocumentation();

            using (var config = JsConfig.BeginScope())
            {
                config.EmitCamelCaseNames = true;

                foreach (var apiResourceDocumentation in documentation.Resources)
                {
                    var schema = JsonSchemaGenerator.Generate(apiResourceDocumentation);
                    var x = schema.ToJson();
                }

                /*var schema = new JsonSchema();
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
                schema.Definitions = defs;*/

                
            }
        }
    }
}

// Type can be enum
// Type for definition is []