// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Servicestack.IntroSpec.Raml;
    using Extensions;

    public class RamlWorkingSet
    {
        public string BasePath { get; }
        public string MediaTypeExtensionPath { get; }
        public IEnumerable<string> AvailablePaths => new [] { BasePath, MediaTypeExtensionPath };

        private readonly List<RamlParameter> ramlParameters = new List<RamlParameter>();

        public RamlWorkingSet(string path)
        {
            path.ThrowIfNullOrEmpty(nameof(path));

            BasePath = path.EnsureStartsWith("/");
            MediaTypeExtensionPath = string.Concat(BasePath.TrimEnd('/'), $"{{{Constants.MediaTypeExtensionKey}}}");
        }

        public void Add(RamlParameter ramlParameter)
        {
            ramlParameters.Add(ramlParameter);
        }

        public IEnumerable<RamlParameter> PathParams
            => ramlParameters?.Where(p => p.IsPathParam) ?? Enumerable.Empty<RamlParameter>();

        public IEnumerable<RamlParameter> NonPathParams
            => ramlParameters?.Where(p => !p.IsPathParam) ?? Enumerable.Empty<RamlParameter>();
    }
}