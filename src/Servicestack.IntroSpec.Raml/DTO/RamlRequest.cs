// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Servicestack.IntroSpec.Raml.DTO
{
    using ServiceStack;
    using ServiceStack.DataAnnotations;
    using ServiceStack.IntroSpec.DTO;

    [Route(Constants.DefaultUri)]
    [Route(Constants.Version08Uri)]
    [Exclude(Feature.Metadata | Feature.ServiceDiscovery)]
    public class RamlRequest : IReturn<RamlResponse>, IFilterableSpecRequest
    {
        public string[] DtoNames { get; set; }
        public string[] Categories { get; set; }
        public string[] Tags { get; set; }
    }
}