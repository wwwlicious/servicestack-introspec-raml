// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
namespace ServiceStack.IntroSpec.Raml.Extensions
{
    using IntroSpec.Extensions;
    using Web;

    public static class RequestExtensions
    {
        private const string RamlHeaderKey = "RamlVersion";

        public static void SetRamlVersion(this IRequest request, string version)
            => request.Items.Add(RamlHeaderKey, version);

        public static string GetRamlVersion(this IRequest request) => request.Items.SafeGet(RamlHeaderKey)?.ToString();

    }
}
