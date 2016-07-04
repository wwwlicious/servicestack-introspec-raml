// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Models
{
    // https://github.com/donaldgray/raml-spec/blob/master/versions/raml-08/raml-08.md#resource-types-and-traits
    public class RamlTrait : RamlMethod
    {
        public string Usage { get; set; }
    }
}