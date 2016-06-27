// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests.Fixtures
{
    using System;
    using Models;
    using Settings;
    using Testing;

    public class AppHostFixture : IDisposable
    {
        public BasicAppHost AppHost { get; set; }

        public const string WebHostUrl = "http://127.0.0.1:8090";
        public const string ServiceName = "testService";

        public AppHostFixture()
        {
            var hostConfig = new HostConfig
            {
                WebHostUrl = WebHostUrl,
                DebugMode = true
            };

            AppHost = new BasicAppHost
            {
                TestMode = true,
                Config = hostConfig,
                ServiceName = ServiceName
            };

            var apiSpecConfig = new ApiSpecConfig
            {
                Contact = new ApiContact { Email = "ronald.macdonald@macdonalds.hq", Name = "ronnie mcd" },
                Description = "great api"
            };

            AppHost.Plugins.Add(new ApiSpecFeature(config => apiSpecConfig));
            AppHost.Init();
            AppHost.Config.WebHostUrl = WebHostUrl;
        }

        public void Dispose() => AppHost?.Dispose();
    }
}