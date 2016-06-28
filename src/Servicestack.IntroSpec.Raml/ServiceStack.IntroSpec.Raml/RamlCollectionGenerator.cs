// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Servicestack.IntroSpec.Raml
{
    using System.Collections.Generic;
    using ServiceStack.IntroSpec.Models;
    using ServiceStack.IntroSpec.Raml.Models;
    using ServiceStack.Logging;

    public class RamlCollectionGenerator
    {
        private readonly ILog log = LogManager.GetLogger(typeof(RamlCollectionGenerator));

        public Dictionary<string, string> FriendlyTypeNames = new Dictionary<string, string>
        {
            { "String", "string" },
            { "Int64", "number" },
            { "Double", "number" },
            { "Float", "number" },
            { "Int16", "integer" },
            { "Int32", "integer" },
            { "Single", "number" },
            { "DateTime", "date" },
            { "Boolean", "boolean" }
        };

        public RamlSpec Generate(ApiDocumentation documentation)
        {
            log.Debug($"Generating Raml Spec for service {documentation.Title}");

            var ramlSpec = new RamlSpec();
            SetBasicInformation(documentation, ramlSpec);
            SetResources(documentation, ramlSpec);

            log.Debug($"Generated Raml Spec for resource {documentation.Title}");
            return ramlSpec;
        }

        private void SetBasicInformation(ApiDocumentation documentation, RamlSpec ramlSpec)
        {
            ramlSpec.Title = documentation.Title;
            ramlSpec.Version = documentation.ApiVersion;
            ramlSpec.BaseUri = documentation.ApiBaseUrl;
        }

        private void SetResources(ApiDocumentation documentation, RamlSpec ramlSpec)
        {
            // Iterate through resources
            foreach (var resource in documentation.Resources)
            {
                log.Debug($"Processing resource {resource.Title}");

                // For each resource iterate through it's actions
                foreach (var action in resource.Actions)
                {
                    log.Debug($"Processing action {action.Verb} for resource {resource.Title}");

                    // For each action, go through relative paths
                    foreach (var path in action.RelativePaths)
                    {
                        log.Debug($"Processing path {path} for action {action.Verb} for resource {resource.Title}");

                        // Check if this path already exists in the map
                        RamlResource ramlResource;
                        bool newResource = false;
                        if (ramlSpec.Resources.TryGetValue(path, out ramlResource))
                            log.Debug($"Found raml resource for path {path} for action {action.Verb} for resource {resource.Title}");
                        else
                        {
                            newResource = true;
                            ramlResource = new RamlResource
                            {
                                DisplayName = resource.Title,
                                Description = resource.Description
                            };
                            log.Debug($"Did not find raml resource for path {path} for action {action.Verb} for resource {resource.Title}");
                        }

                        var method = new RamlMethod { Description = action.Notes };
                        ramlResource.Methods.Add(action.Verb.ToLower(), method);

                        if (newResource)
                            ramlSpec.Resources.Add(path, ramlResource);
                    }
                }
            }
        }
    }
}