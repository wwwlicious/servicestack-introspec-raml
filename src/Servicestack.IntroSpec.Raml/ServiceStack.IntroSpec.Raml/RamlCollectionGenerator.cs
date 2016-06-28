// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Servicestack.IntroSpec.Raml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ServiceStack;
    using ServiceStack.IntroSpec.Extensions;
    using ServiceStack.IntroSpec.Models;
    using ServiceStack.IntroSpec.Raml.Extensions;
    using ServiceStack.IntroSpec.Raml.Models;
    using ServiceStack.Logging;
    using ServiceStack.Support.Markdown;
    using CollectionExtensions = ServiceStack.IntroSpec.Extensions.CollectionExtensions;
    using ReflectionExtensions = ServiceStack.ReflectionExtensions;

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
                            ramlSpec.Resources.Add(path.EnsureStartsWith("/"), ramlResource);

                        // Process path parameters
                        var pathParams = path.GetPathParams();
                        if (!CollectionExtensions.IsNullOrEmpty(pathParams))
                        {
                            foreach (var pathParam in pathParams.Distinct())
                            {
                                // Add path parameters
                                // TODO - handle this not being found
                                var propDetails = resource.Properties.FirstOrDefault(r => r.Id == pathParam);

                                var namedParam = GenerateUriParameter(propDetails);

                                ramlResource.UriParameters.Add(pathParam, namedParam);
                            }
                        }

                        ProcessMediaTypeExtensions(ramlResource, action);
                    }
                }
            }
        }

        /// <summary>
        /// MediaTypeExtension is a reserved path name which specifies that adding known extension is equivalent of sending accept header
        /// e.g. appending .json == accept:application/json
        /// </summary>
        /// <remarks>see https://github.com/donaldgray/raml-spec/blob/master/versions/raml-08/raml-08.md#template-uris-and-uri-parameters </remarks>
        private void ProcessMediaTypeExtensions(RamlResource ramlResource, ApiAction action)
        {
            const string mediaTypeParam = "MediaTypeExtension";
            if (ramlResource.UriParameters.ContainsKey(mediaTypeParam)) return;

            var extensions = new Dictionary<string, string>();
            foreach (var contentType in action.ContentTypes)
            {
                try
                {
                    // Get friendly name
                    var extension = MimeTypes.GetExtension(contentType);
                    if (!extension.Contains(" "))
                        extensions.Add(contentType, extension);

                    log.Debug($"Found extension {extension} for {contentType}");
                }
                catch (NotSupportedException nse)
                {
                    log.Error($"Mime Type {contentType} supported not supported.", nse);
                }
            }

            if (extensions.IsNullOrEmpty()) return;

            // Generates message like "Use .json to specify application/json or .xml to specify text/xml"
            var message = string.Concat("Use ",
                string.Join(" or ", extensions.Select(s => $"{s.Value} to specify {s.Key}")));

            ramlResource.UriParameters.Add(mediaTypeParam,
                new RamlNamedParameter { Enum = extensions.Select(s => s.Value), Description = message });
        }

        // TODO handle other types
        private RamlNamedParameter GenerateUriParameter(ApiPropertyDocumention property)
        {
            var uriParameter = new RamlNamedParameter
            {
                DisplayName = property.Title,
                Description = property.Description,
                Type = FriendlyTypeNames.SafeGet(property.ClrType.ToString(), (string) null)
            };

            if (property.AllowMultiple ?? false)
                uriParameter.Repeat = true;

            if (property.IsRequired ?? false)
                uriParameter.Required = true;

            if (property.Contraints != null)
            {
                if (property.Contraints.Type == ConstraintType.List)
                {
                    uriParameter.Enum = property.Contraints.Values;
                }
                else if (property.Contraints.Type == ConstraintType.Range)
                {
                    uriParameter.Minimum = property.Contraints.Min;
                    uriParameter.Maximum = property.Contraints.Max;
                }
            }

            return uriParameter;
        }
    }
}