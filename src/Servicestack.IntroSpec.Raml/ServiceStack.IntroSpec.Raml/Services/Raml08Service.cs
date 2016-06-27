// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Servicestack.IntroSpec.Raml.Services
{
    using DTO;
    using ServiceStack;
    using ServiceStack.IntroSpec.Raml.Extensions;

#if !DEBUG
    [CacheResponse(MaxAge = 300, Duration = 600)]
#endif
    public class Raml08Service : Service
    {
        private const string RamlVerison = "#%RAML 0.8";

        [AddHeader(ContentType = Constants.RamlMediaType)]
        public object Get(RamlRequest request)
        {
            Request.SetRamlVersion(RamlVerison);
            return "test";
        }
    }
}
