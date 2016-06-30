// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.JsonSchema
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using IntroSpec.Extensions;
    using IntroSpec.Models;

    public static class JsonSchemaGenerator
    {
        // TODO - correct? Make this changeable?

        public static JsonSchema Generate(IApiResourceType resource)
        {
            // Create with default schema
            var schema = new JsonSchema
            {
                Description = $"Schema for {resource.Title}. {resource.Description}",
                Type = resource.Title
            };

            if (resource.Properties.IsNullOrEmpty())
                return schema;

            PopulateBaseFields(schema, resource.Properties);

            return schema;
        }

        // This needs to return a dictionary
        public static IDictionary<string, JsonSchemaDefinition> GetDefinitions(IApiResourceType resource)
        {
            // Get all embedded refs and convert to definitions
            var embeddedResources = resource.Properties.SelectMany(GetPropertiesWithEmbeddedResources).ToList();
            
            var dictionary = new Dictionary<string, JsonSchemaDefinition>();
            if (embeddedResources.IsNullOrEmpty())
                return dictionary;

            foreach (var prop in embeddedResources)
            {
                dictionary.Add(prop.Title, prop.ConvertToDefinition());
            }

            return dictionary;
        }

        public static JsonSchemaDefinition ConvertToDefinition(this ApiPropertyDocumention property)
        {
            var definition = new JsonSchemaDefinition();
            definition.Type = JsonSchemaTypeLookup.GetJsonTypes(property.ClrType, property.IsRequired ?? false);

            // Required is every prop that IsRequired = true

            return definition;
        }

        public static IEnumerable<ApiPropertyDocumention> GetPropertiesWithEmbeddedResources(ApiPropertyDocumention prop)
        {
            if (prop.EmbeddedResource != null)
            {
                yield return prop;

                if (!prop.EmbeddedResource.Properties.IsNullOrEmpty())
                    foreach (var p in prop.EmbeddedResource.Properties.SelectMany(GetPropertiesWithEmbeddedResources))
                        yield return p;
            }
        }

        public static IEnumerable<IApiResourceType> GetEmbeddedResources(ApiPropertyDocumention prop)
        {
            if (prop.EmbeddedResource != null)
            {
                yield return prop.EmbeddedResource;

                if (!prop.EmbeddedResource.Properties.IsNullOrEmpty())
                    foreach (var p in prop.EmbeddedResource.Properties.SelectMany(GetEmbeddedResources))
                        yield return p;
            }
        }

        public static void PopulateBaseFields(IJsonSchemaBase obj, IEnumerable<ApiPropertyDocumention> properties)
        {
            var dict = new Dictionary<string, JsonSchemaProperty>();
            var apiPropertyDocumentions = properties as ApiPropertyDocumention[] ?? properties.ToArray();

            var requiredList = new List<string>(apiPropertyDocumentions.Length);

            foreach (var property in apiPropertyDocumentions)
            {
                var jsonProp = new JsonSchemaProperty();
                jsonProp.Type = JsonSchemaTypeLookup.GetJsonTypes(property.ClrType, property.IsRequired ?? false);

                if (property.IsRequired ?? false)
                    requiredList.Add(property.Title);

                // TODO Will need to know if this is referencing an exernal type first too... Process definitions first???

                dict.Add(property.Title, jsonProp);
            }

            obj.Properties = dict;
            obj.Required = requiredList;
        }
    }
}
