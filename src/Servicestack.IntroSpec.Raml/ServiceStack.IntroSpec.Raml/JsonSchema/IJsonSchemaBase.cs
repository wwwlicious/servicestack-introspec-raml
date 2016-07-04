// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.JsonSchema
{
    using System.Collections.Generic;

    public interface IJsonSchemaBase
    {
        IEnumerable<string> Required { get; set; }

        Dictionary<string, JsonProperty> Properties { get; set; }
    }
}