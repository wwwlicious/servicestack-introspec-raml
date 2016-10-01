// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.JsonSchema
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IntroSpec.Extensions;
    using Logging;

    public class JsonSchemaTypeLookup
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(JsonSchemaTypeLookup));

        private static readonly Dictionary<string, string> friendlyTypeNames = new Dictionary<string, string>
        {
            { "String", "string" },
            { "Double", "number" },
            { "Float", "number" },
            { "Int16", "integer" },
            { "Int32", "integer" },
            { "Int64", "integer" },
            { "UInt16", "integer" },
            { "UInt32", "integer" },
            { "UInt64", "integer" },
            { "Single", "number" },
            { "Boolean", "boolean" }
        };

        private const string Fallback = "object";
        private const string NullType = "null";

        public static IEnumerable<string> GetJsonTypes(Type clrType, bool isRequired = false)
        {
            var isNullableType = clrType.IsNullableType();
            var typeName = clrType.IsGenericType && isNullableType
                               ? clrType.GetGenericArguments().First().Name
                               : clrType.Name;

            var jsonTypeName = friendlyTypeNames.SafeGet(typeName, Fallback);

            log.Debug($"Got json type name {jsonTypeName} for clrType {clrType.Name}");

            if (!isRequired && (clrType.IsClass || isNullableType))
                return new[] { jsonTypeName, NullType };

            return new[] { jsonTypeName };
        }

        public static string GetJsonType(string typeName)
            => friendlyTypeNames.SafeGet(typeName, Fallback);
    }
}