// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace Servicestack.IntroSpec.Raml
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using ServiceStack;
    using ServiceStack.IntroSpec.Extensions;
    using ServiceStack.IntroSpec.Models;
    using ServiceStack.IntroSpec.Raml.Extensions;
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            log.Debug($"Generating Raml Spec for service {documentation.Title}");

            var ramlSpec = new RamlSpec();
            SetBasicInformation(documentation, ramlSpec);
            SetResources(documentation, ramlSpec);

            stopwatch.Stop();
            log.Debug($"Generated Raml Spec for resource {documentation.Title}. Took {stopwatch.ElapsedMilliseconds}ms");
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
                        // TODO Append need to add {mediaTypeExtension} on to path if it's required


                        log.Debug($"Processing path {path} for action {action.Verb} for resource {resource.Title}");

                        // Check if this path already exists in the map
                        bool isNewResource;
                        var ramlResource = GetRamlResource(ramlSpec, path, resource, out isNewResource);
                        
                        ramlResource.UriParameters = GetUriParameters(path, resource, ramlResource, action);

                        var method = GetActionMethod(action, resource, ramlResource);

                        ramlResource.Methods.Add(action.Verb.ToLower(), method);

                        if (isNewResource)
                            ramlSpec.Resources.Add(path.EnsureStartsWith("/"), ramlResource);
                    }
                }
            }
        }

        private RamlMethod GetActionMethod(ApiAction action, ApiResourceDocumentation resource, RamlResource ramlResource)
        {
            var method = new RamlMethod { Description = action.Notes };

            var hasRequestBody = action.Verb.HasRequestBody();
            if (!hasRequestBody)
                method.QueryParameters = ProcessQueryStrings(resource, method, ramlResource.UriParameters?.Select(p => p.Key));
            return method;
        }

        private RamlResource GetRamlResource(RamlSpec ramlSpec, string path, ApiResourceDocumentation resource,
            out bool newResource)
        {
            RamlResource ramlResource;
            newResource = false;
            if (ramlSpec.Resources.TryGetValue(path, out ramlResource))
                log.Debug($"Found raml resource for path {path}");
            else
            {
                newResource = true;
                ramlResource = new RamlResource
                {
                    DisplayName = resource.Title,
                    Description = resource.Description
                };
                log.Debug($"Did not find raml resource for path {path}");
            }
            return ramlResource;
        }

        private Dictionary<string, RamlNamedParameter> GetUriParameters(string path, ApiResourceDocumentation resource, RamlResource ramlResource, ApiAction action)
        {
            // Process path parameters
            var pathParams = path.GetPathParams();
            if (pathParams.IsNullOrEmpty() || resource.Properties.IsNullOrEmpty()) return null;

            var uriParams = ramlResource.UriParameters ?? new Dictionary<string, RamlNamedParameter>();
            foreach (var pathParam in pathParams.Distinct())
            {
                // TODO - handle this not being found
                var propDetails = resource.Properties.FirstOrDefault(r => r.Id == pathParam);

                var namedParam = GenerateUriParameter(propDetails);

                uriParams.Add(pathParam, namedParam);
            }

            ProcessMediaTypeExtensions(action, uriParams);

            return uriParams;
        }

        private Dictionary<string, RamlNamedParameter> ProcessQueryStrings(ApiResourceDocumentation resource, RamlMethod method, IEnumerable<string> uriParamNames)
        {
            if (resource.Properties.IsNullOrEmpty()) return null;

            if (uriParamNames.IsNullOrEmpty())
                uriParamNames = Enumerable.Empty<string>();

            var queryStrings = new Dictionary<string, RamlNamedParameter>();
            
            // Iterate through all params that weren't UriParameters
            foreach (var param in resource.Properties.Where(p => !uriParamNames.Contains(p.Id)))
            {
                var namedParam = GenerateUriParameter(param);

                queryStrings.Add(param.Id, namedParam);
            }

            return queryStrings;
        }

        /// <summary>
        /// MediaTypeExtension is a reserved path name which specifies that adding known extension is equivalent of sending accept header
        /// e.g. appending .json == accept:application/json
        /// </summary>
        /// <remarks>see https://github.com/donaldgray/raml-spec/blob/master/versions/raml-08/raml-08.md#template-uris-and-uri-parameters </remarks>
        private void ProcessMediaTypeExtensions(ApiAction action, Dictionary<string, RamlNamedParameter> uriParams)
        {
            const string mediaTypeParam = "mediaTypeExtension";
            if (uriParams.ContainsKey(mediaTypeParam)) return;

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

            uriParams.Add(mediaTypeParam,
                new RamlNamedParameter { Enum = extensions.Select(s => s.Value), Description = message });
        }

        private RamlNamedParameter GenerateUriParameter(ApiPropertyDocumention property)
        {
            var uriParameter = new RamlNamedParameter
            {
                DisplayName = property.Title,
                Description = property.Description,
                Type = FriendlyTypeNames.SafeGet(property.ClrType.Name, (string) null) // TODO handle other types
            };

            if (property.AllowMultiple ?? false)
                uriParameter.Repeat = true;

            if (property.IsRequired ?? false)
                uriParameter.Required = true;

            if (property.Contraints == null) return uriParameter;

            switch (property.Contraints.Type)
            {
                case ConstraintType.List:
                    uriParameter.Enum = property.Contraints.Values;
                    break;
                case ConstraintType.Range:
                    uriParameter.Minimum = property.Contraints.Min;
                    uriParameter.Maximum = property.Contraints.Max;
                    break;
                default:
                    log.Info($"Constraint type {property.Contraints.Type} unknown");
                    break;
            }

            return uriParameter;
        }
    }
}