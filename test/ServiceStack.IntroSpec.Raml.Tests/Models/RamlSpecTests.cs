// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.Models
{
    using FluentAssertions;
    using Raml.Models;
    using Xunit;

    public class RamlSpecTests
    {
        [Fact]
        public void Ctor_InitialisesResources()
        {
            var spec = new RamlSpec();
            spec.Resources.Should().NotBeNull();
        }
    }
}
