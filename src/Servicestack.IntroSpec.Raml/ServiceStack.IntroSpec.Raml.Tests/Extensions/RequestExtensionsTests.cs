// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.Extensions
{
    using FluentAssertions;
    using Host;
    using Raml.Extensions;
    using Xunit;

    public class RequestExtensionsTests
    {
        private const string RamlVersionKey = "RamlVersion";

        [Fact]
        public void SetRamlVersion_SetsItemsValue()
        {
            const string version = "#%RAML 99";
            var request = new BasicRequest();
            request.SetRamlVersion(version);

            request.Items.Should().ContainKey(RamlVersionKey);
            request.Items[RamlVersionKey].Should().Be(version);
        }

        [Fact]
        public void GetRamlVersion_ReturnsNull_IfNotSet()
        {
            var request = new BasicRequest();
            var version = request.GetRamlVersion();

            version.Should().BeNullOrEmpty();
        }

        [Fact]
        public void GetRamlVersion_ReturnsVersion_IfSet()
        {
            const string setVersion = "#%RAML 99";
            var request = new BasicRequest();
            request.Items.Add(RamlVersionKey, setVersion);

            var version = request.GetRamlVersion();

            version.Should().Be(setVersion);
        }
    }
}
