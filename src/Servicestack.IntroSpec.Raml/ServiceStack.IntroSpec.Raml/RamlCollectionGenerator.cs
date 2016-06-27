// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Servicestack.IntroSpec.Raml
{
    using ServiceStack.IntroSpec.Models;
    using ServiceStack.IntroSpec.Raml.Models;

    public class RamlCollectionGenerator
    {
        public RamlSpec Generate(ApiDocumentation documentation)
        {
            var ramlSpec = new RamlSpec();
            SetBasicInformation(documentation, ramlSpec);

            return ramlSpec;
        }

        private static void SetBasicInformation(ApiDocumentation documentation, RamlSpec ramlSpec)
        {
            ramlSpec.Title = documentation.Title;
            ramlSpec.Version = documentation.ApiVersion;
            ramlSpec.BaseUri = documentation.ApiBaseUrl;
        }
    }
}