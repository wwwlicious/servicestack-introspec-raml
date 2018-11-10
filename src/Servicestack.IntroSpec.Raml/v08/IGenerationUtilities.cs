// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.v08
{
    using System.Collections.Generic;
    using IntroSpec.Models;
    using Models;

    public interface IGenerationUtilities
    {
        RamlNamedParameter GenerateUriParameter(ApiPropertyDocumentation property);
        RamlWorkingSet GenerateWorkingSet(string path, ApiResourceDocumentation resource);
        Dictionary<string, RamlNamedParameter> GetQueryStringLookup(ApiResourceDocumentation resource, RamlWorkingSet ramlWorkingSet);
        bool HasMediaTypeExtension(RamlResource resource);
        void ProcessMediaTypeExtensions(ApiAction action, Dictionary<string, RamlNamedParameter> uriParams);
    }
}