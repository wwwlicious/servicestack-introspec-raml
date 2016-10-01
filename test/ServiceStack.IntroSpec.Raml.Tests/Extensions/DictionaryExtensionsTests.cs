// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.Extensions
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Raml.Extensions;
    using Xunit;

    public class DictionaryExtensionsTests
    {
        [Fact]
        public void SafeAdd_AddsIfKeyNotPresent()
        {
            var dictionary = new Dictionary<string, int>();

            dictionary.SafeAdd("key", 2);

            dictionary["key"].Should().Be(2);
        }

        [Fact]
        public void SafeAdd_DoesNotChange_IfKeyAlreadyPresent()
        {
            var dictionary = new Dictionary<string, int> { { "key", 2 } };
            
            dictionary.SafeAdd("key", 100);

            dictionary["key"].Should().Be(2);
        }
    }
}
