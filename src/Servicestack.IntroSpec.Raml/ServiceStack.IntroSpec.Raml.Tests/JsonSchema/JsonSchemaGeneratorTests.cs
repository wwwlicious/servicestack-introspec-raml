// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.JsonSchema
{
    using System.Collections.Generic;
    using System.Linq;
    using IntroSpec.Models;
    using Raml.JsonSchema;
    using Xunit;

    public class JsonSchemaGeneratorTests
    {
        [Fact]
        public void GetEmbeddedRefs()
        {
            var resource = new ApiResourceDocumentation();
            var props = new List<ApiPropertyDocumention>();
            for (int x = 0; x < 5; x++)
            {
                var prop = new ApiPropertyDocumention();
                prop.Title = x.ToString();
                props.Add(prop);

                if (x == 0 || x == 3)
                {
                    prop.EmbeddedResource = new ApiResourceDocumentation { Title = $"{x}sub" };
                    prop.EmbeddedResource.Properties = new[]
                    {
                        new ApiPropertyDocumention
                        {
                            EmbeddedResource = new ApiResourceDocumentation { Title = $"{x}subsub" }
                        }
                    };
                }
            }
            resource.Properties = props.ToArray();

            var xasdasdasda = resource.Properties.SelectMany(JsonSchemaGenerator.GetEmbeddedResources).ToList();
            foreach (var t in xasdasdasda)
            {
                bool asdx = true;
            }
            string p = "";
            //var definitions = resource.Properties.Select(x => JsonSchemaGenerator.GetEmbeddedResources(x)).ToList();
            //var sdfx = definitions.Count;
        }
    }
}

