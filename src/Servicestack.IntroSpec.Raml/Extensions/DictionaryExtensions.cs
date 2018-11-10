// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Extensions
{
    using System.Collections.Generic;
    using Logging;

    public static class DictionaryExtensions
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DictionaryExtensions));

        public static void SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary.ThrowIfNull(nameof(dictionary));

            TValue val;
            if (!dictionary.TryGetValue(key, out val))
            {
                dictionary.Add(key, value);
                return;
            }

            if (!val.Equals(value))
                log.Info($"Attempted to add key {key} to dictionary but value already exists. Existing value {val} differs from new value {value}");
        }
    }
}
