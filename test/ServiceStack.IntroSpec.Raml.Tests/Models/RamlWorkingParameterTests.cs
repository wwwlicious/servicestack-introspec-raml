// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.Models
{
    using FluentAssertions;
    using Raml.Models;
    using Servicestack.IntroSpec.Raml;
    using Xunit;

    public class RamlWorkingParameterTests
    {
        [Fact]
        public void Create_SetsAllValues()
        {
            const string key = "Key";
            const string type = "number";
            const bool isPathParam = true;
            var param = new RamlNamedParameter();

            var workingParam = RamlWorkingParameter.Create(key, type, isPathParam, param);

            workingParam.Key.Should().Be(key);
            workingParam.Type.Should().Be(type);
            workingParam.IsPathParam.Should().Be(isPathParam);
            workingParam.Value.Should().Be($"val-{type}");
            workingParam.NamedParam.Should().Be(param);
        }
    }
}
