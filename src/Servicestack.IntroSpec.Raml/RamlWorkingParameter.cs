// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Servicestack.IntroSpec.Raml
{
    using ServiceStack.IntroSpec.Raml.Models;

    public class RamlWorkingParameter
    {
        public string Key { get; private set; }
        public string Type { get; private set; }
        public bool IsPathParam { get; private set; }
        public string Value { get; private set; } // TODO - vary this value depending on the type? Make it RamlParameter<T>?
        public RamlNamedParameter NamedParam { get; private set; }

        public static RamlWorkingParameter Create(string key, string type, bool isPathParam, RamlNamedParameter namedParam)
        {
            return new RamlWorkingParameter
            {
                Key = key,
                Type = type,
                IsPathParam = isPathParam,
                Value = $"val-{type}",
                NamedParam = namedParam
            };
        }
    }
}