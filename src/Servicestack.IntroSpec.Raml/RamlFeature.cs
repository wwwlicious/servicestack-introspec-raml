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
    using ServiceStack.IntroSpec.Raml;

    public class RamlFeature : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            if (!appHost.Plugins.Any(p => p is ApiSpecFeature || p is IntroSpecFeature))
                throw new ArgumentException("The IntroSpecFeature plugin from ServiceStack.IntroSpec must be enabled to use the RAML Feature");

            RamlFormat.RegisterSerializer(appHost);
            RegisterServices(appHost);
        }
        
        private void RegisterServices(IAppHost appHost)
        {
            appHost.RegisterService<Raml08Service>();
            appHost.GetPlugin<MetadataFeature>()?.AddPluginLink(Constants.Version08Uri, "RAML 0.8");
        }
    }
}
