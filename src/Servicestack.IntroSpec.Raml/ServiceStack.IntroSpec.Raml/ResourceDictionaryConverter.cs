// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    public class ResourceDictionaryConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(Dictionary<string, RamlResource>);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            throw new NotImplementedException();
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var resources = value as Dictionary<string, RamlResource>;

            var serializer = new Serializer(SerializationOptions.DisableAliases, new CamelCaseNamingConvention());
            foreach (var resource in resources)
            {
                //emitter.Emit(new MappingStart());
                emitter.Emit(new SequenceStart(null, resource.Key, false, SequenceStyle.Any));
                using (var sw = new StringWriter())
                {
                    serializer.Serialize(sw, resource.Value);
                    var s = sw.ToString().Replace("\r\n", "\n");
                    var scalar = new Scalar(null, resource.Key, s, ScalarStyle.Any, false, false);
                    emitter.Emit(scalar);
                }
            }
            //emitter.Emit(new MappingEnd());
            emitter.Emit(new SequenceEnd());
        }
    }
}