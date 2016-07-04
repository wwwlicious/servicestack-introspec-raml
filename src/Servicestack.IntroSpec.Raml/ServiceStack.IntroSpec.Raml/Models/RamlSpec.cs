// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the basic information for RAML output
    /// </summary>
    public class RamlSpec
    {
        // https://github.com/raml-org/raml-spec/blob/master/versions/raml-08/raml-08.md#basic-information
        public string Title { get; set; }
        public string Version { get; set; }
        public string BaseUri { get; set; }
        public string MediaType { get; set; }
        public IEnumerable<string> Protocols { get; set; }

        // https://github.com/raml-org/raml-spec/blob/master/versions/raml-08/raml-08.md#user-documentation
        public RamlDocumentation[] Documentation { get; set; }

        // https://github.com/raml-org/raml-spec/blob/master/versions/raml-08/raml-08.md#resources-and-nested-resources
        public Dictionary<string, RamlResource> Resources { get; } = new Dictionary<string, RamlResource>();

        public Dictionary<string, RamlResourceType> ResourceTypes { get; set; }

        public Dictionary<string, RamlResourceType> Traits { get; set; }

        // TODO SecuritySchemes
        // TODO define reused schemas
        // TODO Uri Parameters
    }
}
