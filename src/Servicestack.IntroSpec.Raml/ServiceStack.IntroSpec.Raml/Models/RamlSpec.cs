// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Models
{
    using System.Collections.Generic;
    using IntroSpec.Extensions;
    using YamlDotNet.Serialization;
    using NamedParameterMap = System.Collections.Generic.Dictionary<string, RamlNamedParameter>;

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

        // TODO - define reused schemas here?? Will there be any reused?
       
        // Uri Parameters

        // https://github.com/raml-org/raml-spec/blob/master/versions/raml-08/raml-08.md#user-documentation
        public RamlDocumentation[] Documentation { get; set; }

        // https://github.com/raml-org/raml-spec/blob/master/versions/raml-08/raml-08.md#resources-and-nested-resources
        public Dictionary<string, RamlResource> Resources { get; } = new Dictionary<string, RamlResource>();

        public Dictionary<string, RamlResourceType> ResourceTypes { get; set; }

        public Dictionary<string, RamlResourceType> Traits { get; set; }

        // TODO SecuritySchemes
    }

    public class RamlDocumentation
    {
        public string Title { get; set; }

        // TODO This may be mardown or !include content
        public string Content { get; set; }
    }

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
        // Key == method
        [YamlIgnore]
        public Dictionary<string, RamlMethod> Methods { get; } = new Dictionary<string, RamlMethod>();

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

    public class RamlMethod : RamlHasBody
    {
        public string[] Protocols { get; set; }

        public NamedParameterMap QueryParameters { get; set; }

        // TODO Headers
        // Key == full header name (x-my-header)
        public NamedParameterMap Headers { get; set; }

        // Key == status code (200, 201 etc)
        public Dictionary<int, RamlHasBody> Responses { get; set; }
    }

    public class RamlHasBody
    {
        public string Description { get; set; }

        public RamlBody Body { get; set; }
    }

    public class RamlBody
    {
        [YamlMember(Alias = "application/json")]
        public RamlSchema JsonSchema { get; set; }

        // TODO - XML/any other schema?
    }

    public class RamlSchema
    {
        public string Schema { get; set; }
    }

    // TODO Named parameters with multiple types - is that possible in SS?
    public class RamlNamedParameter
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public IEnumerable<string> Enum { get; set; }
        public string Pattern { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public object Minimum { get; set; }
        public object Maximum { get; set; }
        public object Example { get; set; }
        public bool Repeat { get; set; }
        public bool Required { get; set; }
        public object Default { get; set; }
    }

    // https://github.com/donaldgray/raml-spec/blob/master/versions/raml-08/raml-08.md#resource-types-and-traits
    public class RamlTrait : RamlMethod
    {
        public string Usage { get; set; }
    }

    // https://github.com/donaldgray/raml-spec/blob/master/versions/raml-08/raml-08.md#resource-types-and-traits
    public class RamlResourceType : RamlResource
    {
        public string Usage { get; set; }
    }
}
