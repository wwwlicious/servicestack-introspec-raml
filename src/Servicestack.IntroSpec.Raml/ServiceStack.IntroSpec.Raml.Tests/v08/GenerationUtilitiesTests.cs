// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.v08
{
    using System;
    using FluentAssertions;
    using IntroSpec.Models;
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
    }
}
