// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests
{
    using System;
    using FakeItEasy;
    using FluentAssertions;
    using IntroSpec.Models;
    using Raml.v08;
    using Servicestack.IntroSpec.Raml;
    using Xunit;

    public class RamlCollectionGeneratorTests
    {
        private readonly IGenerationUtilities utilities;
        private readonly RamlCollectionGenerator ramlCollectionGenerator;

        public RamlCollectionGeneratorTests()
        {
            utilities = A.Fake<IGenerationUtilities>();
            ramlCollectionGenerator = new RamlCollectionGenerator(utilities);
        }

        [Fact]
        public void Ctor_Throws_IfIGenerationUtilitiesNull()
        {
            Action action = () => new RamlCollectionGenerator((IGenerationUtilities) null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: generationUtilities");
        }

        [Fact]
        public void Generate_SetsBasicInformation()
        {
            const string baseUri = "http://127.0.0.1:9999";
            const string version = "v2";
            const string title = "title";

            var apiDocumentation = new ApiDocumentation { ApiBaseUrl = baseUri, ApiVersion = version, Title = title };

            var ramlSpec = ramlCollectionGenerator.Generate(apiDocumentation);
            ramlSpec.Title.Should().Be(title);
            ramlSpec.Version.Should().Be(version);
            ramlSpec.BaseUri.Should().Be(baseUri);
        }
    }
}
