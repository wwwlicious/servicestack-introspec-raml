// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.JsonSchema
{
    using System.Collections.Generic;
    using IntroSpec.Extensions;
    using IntroSpec.Models;

    public class JsonSchemaGenerator
    {
        // TODO - correct? Make this changeable?

        public static JsonSchemaTypeDefinition Generate(IApiResourceType resource)
        {
            /* If there are any embedded resources these will be in the definitions
            will need to keep track of those as they will then need to be referenced */

            // Create with default schema
            var schema = new JsonSchemaTypeDefinition();

            // Add the basic details to this 
            // TODO Should this just be the title
            schema.Description = $"Schema for {resource.Title}. {resource.Description}";

            schema.Type = resource.Title;

            if (resource.Properties.IsNullOrEmpty())
                return schema;
            schema.Properties = new Dictionary<string, JsonSchemaProperty>();
            foreach (var property in resource.Properties)
            {
                var jsonProp = new JsonSchemaProperty();
            }

            return schema;
        }

        // valid values
            /*array A JSON array.
            boolean A JSON boolean.
            integer A JSON number without a fraction or exponent part.
            number Any JSON number. Number includes integer.
            null The JSON null value.
            object A JSON object.
            string A JSON string.*/
    }
}
