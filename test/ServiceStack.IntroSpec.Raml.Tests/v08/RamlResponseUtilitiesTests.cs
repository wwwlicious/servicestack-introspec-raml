// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.v08
{
    using FluentAssertions;
    using IntroSpec.Models;
    using Raml.v08;
    using Xunit;

    public class RamlResponseUtilitiesTests
    {
        [Fact]
        public void GetResponses_ReturnsNull_IfNullStatusCodes()
        {
            var action = new ApiAction { StatusCodes = null };

            RamlResponseUtilities.GetResponses(action, new ApiResourceDocumentation()).Should().BeNull();
        }

        [Fact]
        public void GetResponses_ReturnsNull_IfEmptyStatusCodes()
        {
            var action = new ApiAction { StatusCodes = new StatusCode[0] };

            RamlResponseUtilities.GetResponses(action, new ApiResourceDocumentation()).Should().BeNull();
        }

        [Fact]
        public void GetResponses_ReturnsCorrect_IfNoReturnType()
        {
            var action = new ApiAction { StatusCodes = new[] { (StatusCode) 200, (StatusCode) 500 } };
            var doc = new ApiResourceDocumentation();

            var responses = RamlResponseUtilities.GetResponses(action, doc);
            responses.Count.Should().Be(2);
            responses.Should().ContainKey(200).And.ContainKey(500);
        }

        [Fact]
        public void GetResponses_ReturnsCorrect_IfReturnType()
        {
            var action = new ApiAction { StatusCodes = new[] { (StatusCode)200, (StatusCode)500 } };
            var doc = new ApiResourceDocumentation { ReturnType = new ApiResourceType { TypeName = "Name" } };

            var responses = RamlResponseUtilities.GetResponses(action, doc);
            responses.Count.Should().Be(2);
            responses.Should().ContainKey(200).And.ContainKey(500);
        }

        [Fact]
        public void GetResponses_SetsFullDescription()
        {
            var action = new ApiAction
            {
                StatusCodes = new[] { new StatusCode { Code = 201, Name = "Created", Description = "Thing created" } }
            };
            var doc = new ApiResourceDocumentation();

            var responses = RamlResponseUtilities.GetResponses(action, doc);
            responses.Count.Should().Be(1);
            var response = responses[201];

            response.Description.Should().Be("Created - Thing created");  
        }

        [Theory]
        [InlineData(200)]
        [InlineData(201)]
        [InlineData(299)]
        public void GetResponses_SetsBody_If2xx(int statusCode)
        {
            var action = new ApiAction { StatusCodes = new[] { (StatusCode) statusCode } };
            var doc = new ApiResourceDocumentation { ReturnType = new ApiResourceType { TypeName = "Name" } };

            var responses = RamlResponseUtilities.GetResponses(action, doc);
            responses.Count.Should().Be(1);
            var response = responses[statusCode];

            response.Body.Should().NotBeNull();
        }

        [Theory]
        [InlineData(204)]
        [InlineData(404)]
        [InlineData(501)]
        public void GetResponses_DoesNotSetBody_IfNot2xx(int statusCode)
        {
            var action = new ApiAction { StatusCodes = new[] { (StatusCode)statusCode } };
            var doc = new ApiResourceDocumentation { ReturnType = new ApiResourceType { TypeName = "Name" } };

            var responses = RamlResponseUtilities.GetResponses(action, doc);
            responses.Count.Should().Be(1);
            var response = responses[statusCode];

            response.Body.Should().BeNull();
        }
    }
}
