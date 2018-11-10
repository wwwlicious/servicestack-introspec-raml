// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.Extensions
{
    using FluentAssertions;
    using Raml.Extensions;
    using Xunit;

    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void EnsureStartsWith_ReturnsString_IfNullOrEmpty(string text)
            => text.EnsureStartsWith("foo").Should().Be(text);

        [Fact]
        public void EnsureStartsWith_ReturnsString_IfStartsWith()
        {
            const string text = "foobar";
            text.EnsureStartsWith("foo").Should().Be(text);
        }

        [Fact]
        public void EnsureStartsWith_AppendsValue_IfNotStartsWith()
        {
            const string expected = "foobar";
            const string text = "bar";
            text.EnsureStartsWith("foo").Should().Be(expected);
        }
    }
}
