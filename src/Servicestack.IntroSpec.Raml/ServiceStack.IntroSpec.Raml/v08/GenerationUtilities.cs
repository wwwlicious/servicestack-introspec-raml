// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.v08
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IntroSpec.Extensions;
    using IntroSpec.Models;
    using Logging;
    using Models;
    using Servicestack.IntroSpec.Raml;

    public class GenerationUtilities
    { 
        private static readonly ILog log = LogManager.GetLogger(typeof(GenerationUtilities));

        public static Dictionary<string, string> FriendlyTypeNames = new Dictionary<string, string>
        {
            { "String", "string" },
            { "Int64", "number" },
            { "Double", "number" },
            { "Float", "number" },
            { "Int16", "integer" },
            { "Int32", "integer" },
            { "Single", "number" },
            { "DateTime", "date" },
            { "Boolean", "boolean" }
        };

        public static RamlNamedParameter GenerateUriParameter(ApiPropertyDocumention property)
        {
            property.ThrowIfNull(nameof(property));

            var uriParameter = new RamlNamedParameter
            {
                DisplayName = property.Title,
                Description = property.Description,
                Type = FriendlyTypeNames.SafeGet(property.ClrType.Name, (string)null) // TODO handle other types
            };

            if (property.AllowMultiple ?? false)
                uriParameter.Repeat = true;

            if (property.IsRequired ?? false)
                uriParameter.Required = true;

            if (property.Contraints == null) return uriParameter;

            switch (property.Contraints.Type)
            {
                case ConstraintType.List:
                    uriParameter.Enum = property.Contraints.Values;
                    break;
                case ConstraintType.Range:
                    uriParameter.Minimum = property.Contraints.Min;
                    uriParameter.Maximum = property.Contraints.Max;
                    break;
                default:
                    log.Info($"Constraint type {property.Contraints.Type} unknown");
                    break;
            }

            return uriParameter;
        }

        public static RamlWorkingSet GenerateWorkingSet(string path, ApiResourceDocumentation resource)
        {
            path.ThrowIfNullOrEmpty(nameof(path));
            resource.ThrowIfNull(nameof(resource));

            var data = new RamlWorkingSet(path);

            var pathParams = path.GetPathParams() ?? Enumerable.Empty<string>();

            foreach (var property in resource.Properties ?? Enumerable.Empty<ApiPropertyDocumention>())
            {
                var isUriParam = pathParams.Contains(property.Id, StringComparer.OrdinalIgnoreCase);
                var typeName = FriendlyTypeNames.SafeGet(property.ClrType.Name, (string)null);

                var namedParam = GenerateUriParameter(property);

                var ramlParam = RamlWorkingParameter.Create(property.Id, typeName, isUriParam, namedParam);
                data.Add(ramlParam);
            }

            return data;
        }
    }
}
