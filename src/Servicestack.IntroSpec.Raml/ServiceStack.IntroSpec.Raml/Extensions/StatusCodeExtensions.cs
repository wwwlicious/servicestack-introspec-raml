// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Extensions
{
    using IntroSpec.Models;

    public static class StatusCodeExtensions
    {
        public static bool RenderReturnBody(this StatusCode statusCode)
        {
            var code = statusCode.Code;
            return code >= 200 && code < 300 && code != 204;
        }
    }
}
