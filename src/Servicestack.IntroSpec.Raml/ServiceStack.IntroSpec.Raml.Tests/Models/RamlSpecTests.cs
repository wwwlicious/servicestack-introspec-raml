// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.Models
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Raml.Models;
    using Xunit;
    using YamlDotNet.Serialization;

    public class RamlSpecTests
    {
        [Fact]
        public void Ctor_InitialisesResources()
        {
            var spec = new RamlSpec();
            spec.Resources.Should().NotBeNull();
        }
    }

    public class RamlResourceTests
    {
        private readonly RamlResource spec;

        public RamlResourceTests()
        {
            spec = new RamlResource();
        }

        [Fact]
        public void Ctor_InitialisesMethods()
        {
            spec.Methods.Should().NotBeNull();
        }

        [Fact]
        public void Methods_HasYamlIgnoreAttribute()
        {
            typeof(RamlResource).GetProperty("Methods", typeof(Dictionary<string, RamlMethod>))
                                .FirstAttribute<YamlIgnoreAttribute>().Should().NotBeNull();
        }

        [Fact]
        public void Get_ReturnsGetMethod()
        {
            var method = new RamlMethod();
            spec.Methods.Add("get", method);
            spec.Get.Should().Be(method);
        }

        [Fact]
        public void Get_ReturnsNuull_IfNoGet()
        {
            spec.Get.Should().BeNull();
        }

        [Fact]
        public void Post_ReturnsGetMethod()
        {
            var method = new RamlMethod();
            spec.Methods.Add("post", method);
            spec.Post.Should().Be(method);
        }

        [Fact]
        public void Post_ReturnsNuull_IfNoGet()
        {
            spec.Post.Should().BeNull();
        }

        [Fact]
        public void Put_ReturnsGetMethod()
        {
            var method = new RamlMethod();
            spec.Methods.Add("put", method);
            spec.Put.Should().Be(method);
        }

        [Fact]
        public void Put_ReturnsNuull_IfNoGet()
        {
            spec.Put.Should().BeNull();
        }

        [Fact]
        public void Delete_ReturnsGetMethod()
        {
            var method = new RamlMethod();
            spec.Methods.Add("delete", method);
            spec.Delete.Should().Be(method);
        }

        [Fact]
        public void Delete_ReturnsNuull_IfNoGet()
        {
            spec.Delete.Should().BeNull();
        }

        [Fact]
        public void Options_ReturnsGetMethod()
        {
            var method = new RamlMethod();
            spec.Methods.Add("options", method);
            spec.Options.Should().Be(method);
        }

        [Fact]
        public void Options_ReturnsNuull_IfNoGet()
        {
            spec.Options.Should().BeNull();
        }

        [Fact]
        public void Head_ReturnsGetMethod()
        {
            var method = new RamlMethod();
            spec.Methods.Add("head", method);
            spec.Head.Should().Be(method);
        }

        [Fact]
        public void Head_ReturnsNuull_IfNoGet()
        {
            spec.Head.Should().BeNull();
        }

        [Fact]
        public void Patch_ReturnsGetMethod()
        {
            var method = new RamlMethod();
            spec.Methods.Add("patch", method);
            spec.Patch.Should().Be(method);
        }

        [Fact]
        public void Patch_ReturnsNuull_IfNoGet()
        {
            spec.Patch.Should().BeNull();
        }

        [Fact]
        public void Trace_ReturnsGetMethod()
        {
            var method = new RamlMethod();
            spec.Methods.Add("trace", method);
            spec.Trace.Should().Be(method);
        }

        [Fact]
        public void Trace_ReturnsNuull_IfNoGet()
        {
            spec.Trace.Should().BeNull();
        }

        [Fact]
        public void Connect_ReturnsGetMethod()
        {
            var method = new RamlMethod();
            spec.Methods.Add("connect", method);
            spec.Connect.Should().Be(method);
        }

        [Fact]
        public void Connect_ReturnsNuull_IfNoGet()
        {
            spec.Connect.Should().BeNull();
        }
    }
}
