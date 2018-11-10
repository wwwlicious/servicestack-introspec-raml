// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.v08
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IntroSpec.Extensions;
    using IntroSpec.Models;
    using Logging;
    using Models;
    using Servicestack.IntroSpec.Raml;

    public class GenerationUtilities : IGenerationUtilities
    {
        private readonly ILog log = LogManager.GetLogger(typeof(GenerationUtilities));

        public readonly Dictionary<string, string> FriendlyTypeNames = new Dictionary<string, string>
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

        private readonly HashSet<string> allowedFormats;

        public GenerationUtilities(HashSet<string> allowedFormats)
        {
            this.allowedFormats = allowedFormats ?? new HashSet<string>();
        }

        public RamlNamedParameter GenerateUriParameter(ApiPropertyDocumentation property)
        {
            property.ThrowIfNull(nameof(property));

            var uriParameter = new RamlNamedParameter
            {
                DisplayName = property.Title,
                Description = property.Description,
                Type = FriendlyTypeNames.SafeGet(property.ClrType.Name, (string) null)
            };

            if (property.AllowMultiple ?? false)
                uriParameter.Repeat = true;

            if (property.IsRequired ?? false)
                uriParameter.Required = true;

            if (property.Constraints == null) return uriParameter;

            switch (property.Constraints.Type)
            {
                case ConstraintType.List:
                    uriParameter.Enum = property.Constraints.Values;
                    break;
                case ConstraintType.Range:
                    uriParameter.Minimum = property.Constraints.Min;
                    uriParameter.Maximum = property.Constraints.Max;
                    break;
                default:
                    log.Info($"Constraint type {property.Constraints.Type} unknown");
                    break;
            }

            return uriParameter;
        }

        public RamlWorkingSet GenerateWorkingSet(string path, ApiResourceDocumentation resource)
        {
            path.ThrowIfNullOrEmpty(nameof(path));
            resource.ThrowIfNull(nameof(resource));

            var data = new RamlWorkingSet(path);

            var pathParams = path.GetPathParams() ?? Enumerable.Empty<string>();

            foreach (var property in resource.Properties ?? Enumerable.Empty<ApiPropertyDocumentation>())
            {
                var isUriParam = pathParams.Contains(property.Id, StringComparer.OrdinalIgnoreCase);

                var namedParam = GenerateUriParameter(property);

                var ramlParam = RamlWorkingParameter.Create(property.Id, namedParam.Type, isUriParam, namedParam);
                data.Add(ramlParam);
            }

            return data;
        }

        public bool HasMediaTypeExtension(RamlResource resource)
        {
            if ((resource == null) || resource.UriParameters.IsNullOrEmpty())
                return false;

            return resource.UriParameters.ContainsKey(Constants.MediaTypeExtensionKey);
        }

        public Dictionary<string, RamlNamedParameter> GetQueryStringLookup(ApiResourceDocumentation resource, RamlWorkingSet ramlWorkingSet)
        {
            if (resource.Properties.IsNullOrEmpty()) return null;

            return ramlWorkingSet.NonPathParams.Where(p => !string.IsNullOrEmpty(p.NamedParam.Type)).ToDictionary(
                param => param.Key,
                param => param.NamedParam);
        }

        /// <summary>
        /// MediaTypeExtension is a reserved path name which specifies that adding known extension is equivalent of sending accept header
        /// e.g. appending .json == accept:application/json
        /// </summary>
        /// <remarks>see https://github.com/raml-org/raml-spec/blob/master/versions/raml-08/raml-08.md#template-uris-and-uri-parameters </remarks>
        public void ProcessMediaTypeExtensions(ApiAction action, Dictionary<string, RamlNamedParameter> uriParams)
        {
            action.ThrowIfNull(nameof(action));
            if (uriParams?.ContainsKey(Constants.MediaTypeExtensionKey) ?? false) return;

            var extensions = new Dictionary<string, string>();
            foreach (var contentType in action.ContentTypes)
            {
                try
                {
                    // Get friendly name
                    var extension = MimeTypes.GetExtension(contentType);

                    // TODO - filter out soap requests - any others?
                    // Only add mediaTypeExtensions in the path is an auto-route
                    if (allowedFormats.Contains(extension))
                        extensions.Add(contentType, extension);

                    log.Debug($"Found extension {extension} for {contentType}");
                }
                catch (NotSupportedException nse)
                {
                    log.Warn($"Mime Type {contentType} not supported.", nse);
                }
            }

            if (extensions.IsNullOrEmpty()) return;

            // Generates message like "Use .json to specify application/json or .xml to specify text/xml"
            var message = string.Concat("Use ",
                string.Join(" or ", extensions.Select(s => $"{s.Value} to specify {s.Key}")));

            // Create a dummy parameter for mediaTypeExtensions
            uriParams.Add(Constants.MediaTypeExtensionKey,
                new RamlNamedParameter { Enum = extensions.Select(s => s.Value), Description = message });
        }
    }
}
