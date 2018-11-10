// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.JsonSchema
{
    using System.Runtime.Serialization;

    [DataContract]
    public class JsonSchemaReference
    {
        [DataMember(Name = "$ref")]
        public string Ref { get; private set; }

        // NOTE - all definitions are at same level so can hardcode route.
        public static JsonSchemaReference Create(string definitionName)
            => new JsonSchemaReference { Ref = $"#/definitions/{definitionName}" };
    }
}