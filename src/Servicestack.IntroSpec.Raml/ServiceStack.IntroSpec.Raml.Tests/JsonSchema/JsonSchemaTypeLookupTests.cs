// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.JsonSchema
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Raml.JsonSchema;
    using Xunit;

    public class JsonSchemaTypeLookupTests
    {
        [Theory]
        [InlineData(typeof(int), "integer")]
        [InlineData(typeof(Int64), "integer")]
        [InlineData(typeof(long), "integer")]
        [InlineData(typeof(Int16), "integer")]
        [InlineData(typeof(short), "integer")]
        [InlineData(typeof(ushort), "integer")]
        [InlineData(typeof(float), "number")]
        [InlineData(typeof(double), "number")]
        [InlineData(typeof(Single), "number")]
        [InlineData(typeof(bool), "boolean")]
        [InlineData(typeof(DateTime), "object")]
        public void GetJsonTypes_ReturnsFriendlyNameOnly_ForNonNullableValueTypes(Type type, string expected)
        {
            var result = JsonSchemaTypeLookup.GetJsonTypes(type);
            result.Should().OnlyContain(r => r == expected);
        }

        [Theory]
        [InlineData(typeof(int?), "integer")]
        [InlineData(typeof(DateTime?), "object")]
        public void GetJsonTypes_ReturnsNullAndTypeName_NullableType(Type type, string expected)
        {
            var result = JsonSchemaTypeLookup.GetJsonTypes(type).ToList();
            result.Count.Should().Be(2);
            result.Should().Contain("null").And.Contain(expected);
        }

        [Theory]
        [InlineData(typeof(int?), "integer")]
        [InlineData(typeof(DateTime?), "object")]
        public void GetJsonTypes_ReturnsTypeNameOnly_IfNullableType_AndRequired(Type type, string expected)
        {
            var result = JsonSchemaTypeLookup.GetJsonTypes(type, true).ToList();
            result.Should().OnlyContain(r => r == expected);
        }

        [Fact]
        public void GetJsonTypes_ReturnsNullAndTypeName_ReferenceType()
        {
            var result = JsonSchemaTypeLookup.GetJsonTypes(typeof(JsonSchemaTypeLookup)).ToList();
            result.Count.Should().Be(2);
            result.Should().Contain("null").And.Contain("object");
        }

        [Fact]
        public void GetJsonTypes_ReturnsTypeNameOnly_IfReferenceType_AndRequired()
        {
            var result = JsonSchemaTypeLookup.GetJsonTypes(typeof(JsonSchemaTypeLookup), true).ToList();
            result.Should().OnlyContain(r => r == "object");
        }
    }
}