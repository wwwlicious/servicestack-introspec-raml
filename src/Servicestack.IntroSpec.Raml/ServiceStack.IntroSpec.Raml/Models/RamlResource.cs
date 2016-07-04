// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Models
{
    using System.Collections.Generic;
    using IntroSpec.Extensions;
    using YamlDotNet.Serialization;

    public class RamlResource
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }

        // https://github.com/raml-org/raml-spec/blob/master/versions/raml-08/raml-08.md#resources-and-nested-resources
        // key = relative path
        public Dictionary<string, RamlResource> Resources { get; set; }

        // Key == name of Uri parameter
        public Dictionary<string, RamlNamedParameter> UriParameters { get; set; }

        // https://github.com/raml-org/raml-spec/blob/master/versions/raml-08/raml-08.md#methods
        // Key == method (get, post etc)
        [YamlIgnore]
        public Dictionary<string, RamlMethod> Methods { get; } = new Dictionary<string, RamlMethod>();

        // NOTE - the following is to make rendering valid RAML easier
        public RamlMethod Get => Methods.SafeGet("get", (RamlMethod)null);
        public RamlMethod Post => Methods.SafeGet("post", (RamlMethod)null);
        public RamlMethod Put => Methods.SafeGet("put", (RamlMethod)null);
        public RamlMethod Delete => Methods.SafeGet("delete", (RamlMethod)null);
        public RamlMethod Options => Methods.SafeGet("options", (RamlMethod)null);
        public RamlMethod Head => Methods.SafeGet("head", (RamlMethod)null);
        public RamlMethod Patch => Methods.SafeGet("patch", (RamlMethod)null);
        public RamlMethod Trace => Methods.SafeGet("trace", (RamlMethod)null);
        public RamlMethod Connect => Methods.SafeGet("connect", (RamlMethod)null);
    }
}