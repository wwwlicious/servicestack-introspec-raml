// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Servicestack.IntroSpec.Raml.Services
{
    using DTO;
    using ServiceStack;

#if !DEBUG
    [CacheResponse(MaxAge = 300, Duration = 600)]
#endif
    public class Raml08Service : IService
    {
        [AddHeader(ContentType = Constants.MediaType)]
        public object Get(RamlRequest request)
        {
            return "#%RAML 0.8";
        }
    }
}
