// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Extensions
{
    public static class StringExtensions
    {
        public static string EnsureStartsWith(this string text, string startWith)
        {
            if (string.IsNullOrEmpty(text) || text.StartsWith(startWith))
                return text;

            return $"{startWith}{text}";
        }
    }
}
