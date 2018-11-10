// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Servicestack.IntroSpec.Raml;
    using Extensions;

    /// <summary>
    /// Contains path and non path parameters to make generating RAML response cleaner.
    /// </summary>
    public class RamlWorkingSet
    {
        public string BasePath { get; }
        public string MediaTypeExtensionPath { get; }
        public IEnumerable<string> AvailablePaths => new [] { BasePath, MediaTypeExtensionPath };

        private readonly List<RamlWorkingParameter> ramlParameters = new List<RamlWorkingParameter>();

        public RamlWorkingSet(string path)
        {
            path.ThrowIfNullOrEmpty(nameof(path));

            BasePath = path.EnsureStartsWith("/");
            MediaTypeExtensionPath = string.Concat(BasePath.TrimEnd('/'), $"{{{Constants.MediaTypeExtensionKey}}}");
        }

        public void Add(RamlWorkingParameter ramlParameter)
        {
            ramlParameters.Add(ramlParameter);
        }

        public IEnumerable<RamlWorkingParameter> PathParams
            => ramlParameters?.Where(p => p.IsPathParam) ?? Enumerable.Empty<RamlWorkingParameter>();

        public IEnumerable<RamlWorkingParameter> NonPathParams
            => ramlParameters?.Where(p => !p.IsPathParam) ?? Enumerable.Empty<RamlWorkingParameter>();
    }
}