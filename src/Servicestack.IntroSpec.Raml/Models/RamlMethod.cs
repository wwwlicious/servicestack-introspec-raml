// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Models
{
    using System.Collections.Generic;

    public class RamlMethod
    {
        public string Description { get; set; }

        public string[] Protocols { get; set; }

        public Dictionary<string, RamlNamedParameter> QueryParameters { get; set; }

        public RamlBody Body { get; set; }

        // TODO Headers
        // Key == full header name (x-my-header)
        public Dictionary<string, RamlNamedParameter> Headers { get; set; }

        // Key == status code (200, 201 etc)
        public Dictionary<int, RamlResponse> Responses { get; set; }
    }
}