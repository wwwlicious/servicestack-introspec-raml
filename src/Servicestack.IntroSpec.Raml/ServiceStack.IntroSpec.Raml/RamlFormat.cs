// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml
{
    using System;
    using System.IO;
    using Extensions;
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
            appHost.ContentTypes.Register(Constants.RamlMediaType, Serialize, null);
        }

        private static void Serialize(IRequest requestContext, object dto, Stream outputStream)
        {
            if (dto == null) return;

            try
            {
                var serializer = new Serializer();

                using (var writer = new StreamWriter(outputStream))
                {
                    writer.WriteLine(requestContext.GetRamlVersion());
                    serializer.Serialize(writer, dto);
                }
            }
            catch (Exception ex)
            {
                Log.Error(
                    $"Error serialising {dto} to yaml with content type {Constants.RamlMediaType}. Request uri: {requestContext.AbsoluteUri}",
                    ex);
            }
        }
    }
}
