// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Models
{
    using System;

    /// <summary>
    /// Represents the basic information for RAML output
    /// </summary>
    /// <remarks>See https://github.com/raml-org/raml-spec/blob/master/versions/raml-08/raml-08.md#basic-information</remarks>
    public class RamlSpec
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string BaseUri { get; set; }
        public string MediaType { get; set; }
        public string[] Protocols { get; set; }
        public string[] Schemas { get; set; }

        // Uri Parameters

        // User documentation - Could link to MD files? Circular dependency?

        // Resources
    }
}
