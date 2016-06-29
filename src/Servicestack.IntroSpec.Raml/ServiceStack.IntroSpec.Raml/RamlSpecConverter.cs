// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml
{
    using System.Collections.Generic;
    using System.Reflection;
    using Models;

    public static class RamlSpecConverter
    {
        public static Dictionary<string, object> ConvertToSerializableDictionary(this RamlSpec spec)
        {
            var output = new Dictionary<string, object>();
            var props = typeof(RamlSpec).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var resourcesType = typeof(Dictionary<string, RamlResource>);
            foreach (var prop in props)
            {
                if (prop.PropertyType == resourcesType)
                {
                    ProcessResources(spec, output);
                    continue;
                }

                var value = prop.GetValue(spec);
                if (value != null)
                    output.Add(prop.Name.ToCamelCase(), value);
            }

            return output;
        }

        private static void ProcessResources(RamlSpec spec, Dictionary<string, object> output)
        {
            foreach (var resource in spec.Resources)
                output.Add(resource.Key, resource.Value);
        }
    }
}