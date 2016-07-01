// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.DTO
{
    using System.Linq;
    using System.Reflection;
    using DataAnnotations;
    using FluentAssertions;
    using Servicestack.IntroSpec.Raml.DTO;
    using Xunit;

    public class RamlRequestTests
    {
        [Fact]
        public void HasExcludeAttribute()
            =>
                typeof(RamlRequest).FirstAttribute<ExcludeAttribute>().Feature.Should().Be(Feature.Metadata |
                                                                                           Feature.ServiceDiscovery);

        [Fact]
        public void HasRouteAttribute()
        {
            var routes = typeof(RamlRequest).GetCustomAttributes<RouteAttribute>().ToList();

            routes[0].Path.Should().Be("/spec/raml");
            routes[1].Path.Should().Be("/spec/raml/0.8");
        }
    }
}
