// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.v08
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using IntroSpec.Models;
    using Raml.Models;
    using Raml.v08;
    using Xunit;

    public class GenerationUtilitiesTests
    {
        private readonly GenerationUtilities generator;
        private readonly GenerationUtilities generatorWithExtensions;

        public GenerationUtilitiesTests()
        {
            generator = new GenerationUtilities(null);
            generatorWithExtensions = new GenerationUtilities(new HashSet<string> { ".json", ".xml", ".jsv" });
        }

        [Fact]
        public void GenerateUriParameters_Throws_IfPropertyNull()
        {
            Action action = () => generator.GenerateUriParameter(null);
            action.ShouldThrow<ArgumentException>().WithMessage("Value cannot be null.\r\nParameter name: property");
        }

        [Fact]
        public void GenerateUriParameters_SetsBasicParams()
        {
            const string displayName = "foo";
            const string description = "bar";
            var prop = new ApiPropertyDocumention { Description = description, Title = displayName, ClrType = typeof(string) };

            var result = generator.GenerateUriParameter(prop);
            result.Description.Should().Be(description);
            result.DisplayName.Should().Be(displayName);
        }

        [Theory]
        [InlineData(typeof(string), "string")]
        [InlineData(typeof(float), "number")]
        [InlineData(typeof(Int64), "number")]
        [InlineData(typeof(long), "number")]
        [InlineData(typeof(double), "number")]
        [InlineData(typeof(DateTime), "date")]
        [InlineData(typeof(bool), "boolean")]
        [InlineData(typeof(GenerationUtilities), null)]
        public void GenerateUriParameters_SetsType(Type type, string expected)
        {
            var prop = new ApiPropertyDocumention { ClrType = type };
            var result = generator.GenerateUriParameter(prop);
            result.Type.Should().Be(expected);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(null, false)]
        [InlineData(true, true)]
        public void GenerateUriParameters_SetsAllowMultiple(bool? allowMultiple, bool repeat)
        {
            var prop = new ApiPropertyDocumention { ClrType = typeof(int), AllowMultiple = allowMultiple };
            var result = generator.GenerateUriParameter(prop);
            result.Repeat.Should().Be(repeat);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(null, false)]
        [InlineData(true, true)]
        public void GenerateUriParameters_SetsRequired(bool? isRequired, bool required)
        {
            var prop = new ApiPropertyDocumention { ClrType = typeof(int), IsRequired = isRequired };
            var result = generator.GenerateUriParameter(prop);
            result.Required.Should().Be(required);
        }

        [Fact]
        public void GenerateUriParameters_HandlesNullConstraints()
        {
            var prop = new ApiPropertyDocumention { ClrType = typeof(int), Contraints = null };

            Action action = () => generator.GenerateUriParameter(prop);
            action.ShouldNotThrow<ArgumentNullException>();
        }

        [Fact]
        public void GenerateUriParameters_SetsEnumValues_IfListConstraint()
        {
            var validValues = new[] { "foo", "bar", "baz" };
            const int min = 10;
            const int max = 100;
            var constraint = new PropertyConstraint
            {
                Type = ConstraintType.List,
                Values = validValues,
                Min = min,
                Max = max
            };

            var prop = new ApiPropertyDocumention { ClrType = typeof(int), Contraints = constraint };
            var result = generator.GenerateUriParameter(prop);

            result.Enum.Should().BeEquivalentTo(validValues);
            result.Minimum.Should().BeNull();
            result.Maximum.Should().BeNull();
        }

        [Fact]
        public void GenerateUriParameters_SetsMinAndMax_IfRangeConstraint()
        {
            var validValues = new[] { "foo", "bar", "baz" };
            const int min = 10;
            const int max = 100;
            var constraint = new PropertyConstraint
            {
                Type = ConstraintType.Range,
                Values = validValues,
                Min = min,
                Max = max
            };

            var prop = new ApiPropertyDocumention { ClrType = typeof(int), Contraints = constraint };
            var result = generator.GenerateUriParameter(prop);

            result.Enum.Should().BeNullOrEmpty();
            result.Minimum.Should().Be(min);
            result.Maximum.Should().Be(max);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GenerateWorkingSet_Throws_IfPathNullOrEmpty(string path)
        {
            Action action = () => generator.GenerateWorkingSet(path, new ApiResourceDocumentation());
            action.ShouldThrow<ArgumentException>().WithMessage("Value cannot be null.\r\nParameter name: path");
        }

        [Fact]
        public void GenerateWorkingSet_Throws_IfResourceNull()
        {
            Action action = () => generator.GenerateWorkingSet("/api", null);
            action.ShouldThrow<ArgumentException>().WithMessage("Value cannot be null.\r\nParameter name: resource");
        }

        [Fact]
        public void GenerateWorkingSet_HandlesNoProperties()
        {
            var ws = generator.GenerateWorkingSet("/api", new ApiResourceDocumentation());
            ws.PathParams.Should().BeEmpty();
            ws.NonPathParams.Should().BeEmpty();
        }

        [Theory]
        [InlineData("/api")]
        [InlineData("/api/{pathParam}")]
        [InlineData("/api/{pathParam1}/{pathParam2}")]
        public void GenerateWorkingSet_AddsParameterPerProperty(string path)
        {
            var ws = generator.GenerateWorkingSet(path,
                new ApiResourceDocumentation
                {
                    Properties =
                        new[]
                        {
                            new ApiPropertyDocumention { ClrType = typeof(string) },
                            new ApiPropertyDocumention { ClrType = typeof(string) }
                        }
                });
            (ws.PathParams.Count() + ws.NonPathParams.Count()).Should().Be(2);
        }

        [Fact]
        public void HasMediaTypeExtension_False_IfResourceNull()
        {
            RamlResource resource = null;
            generator.HasMediaTypeExtension(resource).Should().BeFalse();
        }

        [Fact]
        public void HasMediaTypeExtension_False_IfUriParametersNull()
        {
            var resource = new RamlResource { UriParameters = null };
            generator.HasMediaTypeExtension(resource).Should().BeFalse();
        }

        [Fact]
        public void HasMediaTypeExtension_False_IfUriParametersEmpty()
        {
            var resource = new RamlResource { UriParameters = new Dictionary<string, RamlNamedParameter>() };
            generator.HasMediaTypeExtension(resource).Should().BeFalse();
        }


        [Fact]
        public void HasMediaTypeExtension_False_IfNoMediaTypeExtensionParam()
        {
            var resource = new RamlResource
            {
                UriParameters = new Dictionary<string, RamlNamedParameter>
                {
                    { "Foo", new RamlNamedParameter() },
                    { "Bar", new RamlNamedParameter() }
                }
            };
            generator.HasMediaTypeExtension(resource).Should().BeFalse();
        }

        [Fact]
        public void HasMediaTypeExtension_True_IfMediaTypeExtensionParam()
        {
            var resource = new RamlResource
            {
                UriParameters = new Dictionary<string, RamlNamedParameter>
                {
                    { "Foo", new RamlNamedParameter() },
                    { "mediaTypeExtension", new RamlNamedParameter() }
                }
            };
            generator.HasMediaTypeExtension(resource).Should().BeTrue();
        }

        [Fact]
        public void ProcessQueryStrings_ReturnsNull_IfNullProperties()
        {
            generator.GetQueryStringLookup(new ApiResourceDocumentation(), new RamlWorkingSet("/api"))
                .Should().BeNull();
        }

        [Fact]
        public void ProcessQueryStrings_ReturnsNull_IfEmptyProperties()
        {
            var apiResourceDocumentation = new ApiResourceDocumentation { Properties = new ApiPropertyDocumention[0] };
            generator.GetQueryStringLookup(apiResourceDocumentation, new RamlWorkingSet("/api"))
                .Should().BeNull();
        }

        [Fact]
        public void ProcessMediaTypeExtensions_DoesNotUpdateUriParams_IfMediaTypeExtensionExists()
        {
            var uriParams = new Dictionary<string, RamlNamedParameter>
            {
                { "mediaTypeExtension", null }
            };

            generator.ProcessMediaTypeExtensions(null, uriParams);
            uriParams.Count.Should().Be(1);
        }

        [Fact]
        public void ProcessMediaTypeExtensions_IfNoKnownFileTypes()
        {
            var action = new ApiAction { ContentTypes = new[] { "application/hal+json", "text/yaml" } };
            var uriParams = new Dictionary<string, RamlNamedParameter>();

            generatorWithExtensions.ProcessMediaTypeExtensions(action, uriParams);
            uriParams.Should().BeEmpty();
        }

        [Fact]
        public void ProcessMediaTypeExtensions_AddsMediaTypeExtensionParameter()
        {
            var action = new ApiAction { ContentTypes = new[] { "application/json", "text/xml" } };
            var uriParams = new Dictionary<string, RamlNamedParameter>();

            generatorWithExtensions.ProcessMediaTypeExtensions(action, uriParams);
            uriParams.Should().ContainKey("mediaTypeExtension");
        }

        [Fact]
        public void ProcessMediaTypeExtensions_AddsKnownExtensionTypes()
        {
            var action = new ApiAction { ContentTypes = new[] { "application/json", "text/xml" } };
            var uriParams = new Dictionary<string, RamlNamedParameter>();

            generatorWithExtensions.ProcessMediaTypeExtensions(action, uriParams);
            var param = uriParams["mediaTypeExtension"];
            param.Enum.Should().Contain(".xml").And.Contain(".json");
        }

        [Fact]
        public void ProcessMediaTypeExtensions_SetsCorrectDescritiption()
        {
            var action = new ApiAction { ContentTypes = new[] { "application/json", "text/xml" } };
            var uriParams = new Dictionary<string, RamlNamedParameter>();

            generatorWithExtensions.ProcessMediaTypeExtensions(action, uriParams);
            var param = uriParams["mediaTypeExtension"];
            param.Description.Should().Be("Use .json to specify application/json or .xml to specify text/xml");
        }
    }
}
