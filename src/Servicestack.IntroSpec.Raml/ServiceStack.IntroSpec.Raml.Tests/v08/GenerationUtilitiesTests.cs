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
        [Fact]
        public void GenerateUriParameters_Throws_IfPropertyNull()
        {
            Action action = () => GenerationUtilities.GenerateUriParameter(null);
            action.ShouldThrow<ArgumentException>().WithMessage("Value cannot be null.\r\nParameter name: property");
        }

        [Fact]
        public void GenerateUriParameters_SetsBasicParams()
        {
            const string displayName = "foo";
            const string description = "bar";
            var prop = new ApiPropertyDocumention { Description = description, Title = displayName, ClrType = typeof(string) };

            var result = GenerationUtilities.GenerateUriParameter(prop);
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
            var result = GenerationUtilities.GenerateUriParameter(prop);
            result.Type.Should().Be(expected);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(null, false)]
        [InlineData(true, true)]
        public void GenerateUriParameters_SetsAllowMultiple(bool? allowMultiple, bool repeat)
        {
            var prop = new ApiPropertyDocumention { ClrType = typeof(int), AllowMultiple = allowMultiple };
            var result = GenerationUtilities.GenerateUriParameter(prop);
            result.Repeat.Should().Be(repeat);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(null, false)]
        [InlineData(true, true)]
        public void GenerateUriParameters_SetsRequired(bool? isRequired, bool required)
        {
            var prop = new ApiPropertyDocumention { ClrType = typeof(int), IsRequired = isRequired };
            var result = GenerationUtilities.GenerateUriParameter(prop);
            result.Required.Should().Be(required);
        }

        [Fact]
        public void GenerateUriParameters_HandlesNullConstraints()
        {
            var prop = new ApiPropertyDocumention { ClrType = typeof(int), Contraints = null };

            Action action = () => GenerationUtilities.GenerateUriParameter(prop);
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
            var result = GenerationUtilities.GenerateUriParameter(prop);

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
            var result = GenerationUtilities.GenerateUriParameter(prop);

            result.Enum.Should().BeNullOrEmpty();
            result.Minimum.Should().Be(min);
            result.Maximum.Should().Be(max);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GenerateWorkingSet_Throws_IfPathNullOrEmpty(string path)
        {
            Action action = () => GenerationUtilities.GenerateWorkingSet(path, new ApiResourceDocumentation());
            action.ShouldThrow<ArgumentException>().WithMessage("Value cannot be null.\r\nParameter name: path");
        }

        [Fact]
        public void GenerateWorkingSet_Throws_IfResourceNull()
        {
            Action action = () => GenerationUtilities.GenerateWorkingSet("/api", null);
            action.ShouldThrow<ArgumentException>().WithMessage("Value cannot be null.\r\nParameter name: resource");
        }

        [Fact]
        public void GenerateWorkingSet_HandlesNoProperties()
        {
            var ws = GenerationUtilities.GenerateWorkingSet("/api", new ApiResourceDocumentation());
            ws.PathParams.Should().BeEmpty();
            ws.NonPathParams.Should().BeEmpty();
        }

        [Theory]
        [InlineData("/api")]
        [InlineData("/api/{pathParam}")]
        [InlineData("/api/{pathParam1}/{pathParam2}")]
        public void GenerateWorkingSet_AddsParameterPerProperty(string path)
        {
            var ws = GenerationUtilities.GenerateWorkingSet(path,
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
            resource.HasMediaTypeExtension().Should().BeFalse();
        }

        [Fact]
        public void HasMediaTypeExtension_False_IfUriParametersNull()
        {
            var resource = new RamlResource { UriParameters = null };
            resource.HasMediaTypeExtension().Should().BeFalse();
        }

        [Fact]
        public void HasMediaTypeExtension_False_IfUriParametersEmpty()
        {
            var resource = new RamlResource { UriParameters = new Dictionary<string, RamlNamedParameter>() };
            resource.HasMediaTypeExtension().Should().BeFalse();
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
            resource.HasMediaTypeExtension().Should().BeFalse();
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
            resource.HasMediaTypeExtension().Should().BeTrue();
        }
    }
}
