// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.Extensions
{
    using FluentAssertions;
    using IntroSpec.Models;
    using Raml.Extensions;
    using Xunit;

    public class StatusCodeExtensionsTests
    {
        [Theory]
        [InlineData(200)]
        [InlineData(201)]
        public void RenderReturnBody_True_If2xx(int statusCode)
        {
            var sc = (StatusCode)statusCode;
            sc.RenderReturnBody().Should().BeTrue();
        }

        [Theory]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(403)]
        [InlineData(500)]
        [InlineData(503)]
        [InlineData(204)]
        public void RenderReturnBody_False_IfNon2xx_Or204(int statusCode)
        {
            var sc = (StatusCode)statusCode;
            sc.RenderReturnBody().Should().BeFalse();
        }

        [Fact]
        public void GetFullDescription_ReturnsName_IfNoDescription()
        {
            var sc = new StatusCode { Code = 500, Name = "Internal Server Error" };
            sc.GetFullDescription().Should().Be("Internal Server Error");
        }

        [Fact]
        public void GetFullDescription_ReturnsNameAndDescription_IfHasDescription()
        {
            var sc = new StatusCode { Code = 500, Name = "Internal Server Error", Description = "Uh-oh" };
            sc.GetFullDescription().Should().Be("Internal Server Error - Uh-oh");
        }
    }
}
