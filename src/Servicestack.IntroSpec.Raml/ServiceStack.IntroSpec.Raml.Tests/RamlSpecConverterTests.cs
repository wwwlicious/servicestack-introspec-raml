// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests
{
    using FluentAssertions;
    using Raml.Models;
    using Xunit;

    public class RamlSpecConverterTests
    {
        [Fact]
        public void ConvertToSerializableDictionary_ReturnsEmptyDictionary_IfSpecNull()
        {
            RamlSpec spec = null;
            spec.ConvertToSerializableDictionary().Should().BeEmpty();
        }

        [Fact]
        public void ConvertToSerializableDictionary_PopulatesBasicProps()
        {
            const string baseUri = "http://127.0.0.1:9999";
            const string version = "v2";
            const string title = "title";

            var spec = new RamlSpec
            {
                Title = title,
                Version = version,
                BaseUri = baseUri
            };

            var dict = spec.ConvertToSerializableDictionary();
            dict["title"].Should().Be(title);
            dict["version"].Should().Be(version);
            dict["baseUri"].Should().Be(baseUri);
        }

        [Fact]
        public void ConvertToSerializableDictionary_IgnoresNullProps()
        {
            const string baseUri = "http://127.0.0.1:9999";
            const string version = "v2";

            var spec = new RamlSpec
            {
                Version = version,
                BaseUri = baseUri
            };

            var dict = spec.ConvertToSerializableDictionary();
            dict.Should().NotContainKey("title");
            dict["version"].Should().Be(version);
            dict["baseUri"].Should().Be(baseUri);
        }

        [Fact]
        public void ConvertToSerializableDictionary_FlattensResourceDictioanry()
        {
            var spec = new RamlSpec();
            spec.Resources.Add("/api/hip", new RamlResource());
            spec.Resources.Add("/api/hop", new RamlResource());
            spec.Resources.Add("/api/hurray", new RamlResource());

            var dict = spec.ConvertToSerializableDictionary();
            dict.Should()
                .NotContainKey("resources")
                .And.ContainKey("/api/hip")
                .And.ContainKey("/api/hop")
                .And.ContainKey("/api/hurray");
        }
    }
}
