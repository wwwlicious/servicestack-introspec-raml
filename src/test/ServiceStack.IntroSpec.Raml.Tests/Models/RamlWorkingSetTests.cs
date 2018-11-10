// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.Models
{
    using System;
    using FluentAssertions;
    using Raml.Models;
    using Servicestack.IntroSpec.Raml;
    using Xunit;

    public class RamlWorkingSetTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Ctor_ThrowsIfNullOrEmptyPath(string path)
        {
            Action action = () => new RamlWorkingSet(path);
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("api/foo", "/api/foo")]
        [InlineData("/api/foo", "/api/foo")]
        [InlineData("/api/{foo}", "/api/{foo}")]
        [InlineData("api/{foo}", "/api/{foo}")]
        [InlineData("/api/{foo}/bar", "/api/{foo}/bar")]
        [InlineData("api/{foo}/bar", "/api/{foo}/bar")]
        public void Ctor_SetsBasePath(string path, string expected)
            => new RamlWorkingSet(path).BasePath.Should().Be(expected);

        [Theory]
        [InlineData("api/foo", "/api/foo{mediaTypeExtension}")]
        [InlineData("/api/foo/", "/api/foo{mediaTypeExtension}")]
        [InlineData("/api/{foo}/", "/api/{foo}{mediaTypeExtension}")]
        [InlineData("api/{foo}/", "/api/{foo}{mediaTypeExtension}")]
        [InlineData("/api/{foo}/bar", "/api/{foo}/bar{mediaTypeExtension}")]
        [InlineData("api/{foo}/bar/", "/api/{foo}/bar{mediaTypeExtension}")]
        public void Ctor_MediaTypeExtensionPath(string path, string expected)
            => new RamlWorkingSet(path).MediaTypeExtensionPath.Should().Be(expected);

        [Fact]
        public void PathParams_ReturnsEmpty_IfNoValues() 
            => new RamlWorkingSet("/path").PathParams.Should().BeEmpty();

        [Fact]
        public void NonPathParams_ReturnsEmpty_IfNoValues()
            => new RamlWorkingSet("/path").NonPathParams.Should().BeEmpty();

        [Fact]
        public void PathParams_ReturnsPathParams()
        {
            var param = RamlWorkingParameter.Create("key", "type", true, null);
            var param2 = RamlWorkingParameter.Create("key", "type", true, null);
            var param3 = RamlWorkingParameter.Create("key", "type", false, null);

            var workingSet = new RamlWorkingSet("/path");
            workingSet.Add(param);
            workingSet.Add(param2);
            workingSet.Add(param3);

            workingSet.PathParams.Should().OnlyContain(x => x.IsPathParam);
        }

        [Fact]
        public void PathParams_ReturnsNonPathParams()
        {
            var param = RamlWorkingParameter.Create("key", "type", true, null);
            var param2 = RamlWorkingParameter.Create("key", "type", false, null);
            var param3 = RamlWorkingParameter.Create("key", "type", false, null);

            var workingSet = new RamlWorkingSet("/path");
            workingSet.Add(param);
            workingSet.Add(param2);
            workingSet.Add(param3);

            workingSet.NonPathParams.Should().OnlyContain(x => !x.IsPathParam);
        }
    }
}
