// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.v08
{
    using System.Collections.Generic;
    using IntroSpec.Extensions;
    using IntroSpec.Models;
    using Logging;
    using Models;

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
    }
}
