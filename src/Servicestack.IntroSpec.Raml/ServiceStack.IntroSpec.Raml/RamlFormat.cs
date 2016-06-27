// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml
{
    using System;
    using System.IO;
    using Logging;
    using Servicestack.IntroSpec.Raml;
    using Web;
    using YamlDotNet.Serialization;

    /// <summary>
    /// Adds serialisation support for RAML (yaml) files
    /// </summary>
    public class RamlFormat
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RamlFormat));

        public static void RegisterSerializer(IAppHost appHost)
        {
            // NOTE is this the correct SerializationOptions?
            appHost.ContentTypes.Register(Constants.MediaType, SerializeYaml, null);
        }

        private static void SerializeYaml(IRequest requestContext, object dto, Stream outputStream)
        {
            if (dto == null) return;

            try
            {
                var yamlSerializer = new Serializer(SerializationOptions.EmitDefaults);
                var writer = new StreamWriter(outputStream);
                yamlSerializer.Serialize(writer, dto);
            }
            catch (Exception ex)
            {
                Log.Error(
                    $"Error serialising {dto} to yaml with content type {Constants.MediaType}. Request uri: {requestContext.AbsoluteUri}",
                    ex);
            }
        }
    }
}
