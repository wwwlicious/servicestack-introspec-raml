// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.v08
{
    using System.Collections.Generic;
    using Extensions;
    using IntroSpec.Extensions;
    using IntroSpec.Models;
    using JsonSchema;
    using Logging;
    using Models;

    public static class RamlResponseUtilities
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RamlResponseUtilities));

        public static Dictionary<int, RamlResponse> GetResponses(ApiAction action, ApiResourceDocumentation resource)
        {
            if (action.StatusCodes.IsNullOrEmpty())
            {
                log.Info($"Resource {resource.Title} has no status codes.");
                return null;
            }

            var returnType = resource.ReturnType;
            var hasReturnType = returnType != null;
            var responses = new Dictionary<int, RamlResponse>();
            foreach (var statusCode in action.StatusCodes)
            {
                var ramlHasBody = CreateRamlResponseBody(statusCode, hasReturnType, returnType);

                log.Debug($"Adding status code {statusCode.Code} with return type {returnType?.TypeName ?? "<no return type>"}");
                responses.Add(statusCode.Code, ramlHasBody);
            }

            return responses;
        }

        private static RamlResponse CreateRamlResponseBody(StatusCode statusCode, bool hasReturnType, ApiResourceType returnType)
        {
            var ramlHasBody = new RamlResponse { Description = statusCode.GetFullDescription() };

            if (hasReturnType && statusCode.RenderReturnBody())
            {
                ramlHasBody.Body = new RamlBody
                {
                    JsonSchema = new RamlSchema { Schema = JsonSchemaGenerator.Generate(returnType).ToJson() }
                };
            }
            return ramlHasBody;
        }
    }
}
