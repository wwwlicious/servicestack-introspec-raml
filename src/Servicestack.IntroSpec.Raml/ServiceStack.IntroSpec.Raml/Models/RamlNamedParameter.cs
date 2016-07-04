// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Models
{
    using System.Collections.Generic;

    public class RamlNamedParameter
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public IEnumerable<string> Enum { get; set; }
        public string Pattern { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public object Minimum { get; set; }
        public object Maximum { get; set; }
        public object Example { get; set; }
        public bool Repeat { get; set; }
        public bool Required { get; set; }
        public object Default { get; set; }

        // TODO Named parameters with multiple types - is that possible in SS?
    }
}