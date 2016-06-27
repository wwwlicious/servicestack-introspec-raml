// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Servicestack.IntroSpec.Raml
{
    using System;
    using System.Linq;
    using Services;
    using ServiceStack;
    using ServiceStack.IntroSpec;

    public class RamlFeature : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            if (!appHost.Plugins.Any(p => p is ApiSpecFeature))
                throw new ArgumentException("The ApiSpecFeature must be enabled to use the RAML Feature");

            RegisterServices(appHost);
        }

        private void RegisterServices(IAppHost appHost)
        {
            var metadataFeature = appHost.GetPlugin<MetadataFeature>();

            appHost.RegisterService<Raml08Service>();

            metadataFeature.AddPluginLink(Constants.Version08Uri, "RAML 0.8");
        }
    }
}
