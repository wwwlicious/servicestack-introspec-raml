// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests
{
    using System;
    using FakeItEasy;
    using Fixtures;
    using FluentAssertions;
    using Servicestack.IntroSpec.Raml;
    using Servicestack.IntroSpec.Raml.Services;
    using Xunit;

    [Collection("AppHost")]
    public class RamlFeatureTests
    {
        private readonly RamlFeature feature;
        private readonly AppHostFixture fixture;

        public RamlFeatureTests(AppHostFixture fixture)
        {
            this.fixture = fixture;
            feature = new RamlFeature();
        }

        [Fact]
        public void Register_Throws_IfNoMetadata()
        {
            Action action = () => feature.Register(A.Fake<IAppHost>());
            action.ShouldThrow<ArgumentException>()
                  .WithMessage("The ApiSpecFeature from ServiceStack.IntroSpec must be enabled to use the RAML Feature");
        }

        [Fact]
        public void Register_RegistersService()
        {
            feature.Register(fixture.AppHost);
            fixture.AppHost.Container.TryResolve<Raml08Service>().Should().NotBeNull();
        }
    }
}
