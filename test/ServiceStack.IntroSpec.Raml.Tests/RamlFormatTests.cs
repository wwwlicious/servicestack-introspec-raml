// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this 
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace ServiceStack.IntroSpec.Raml.Tests
{
    using System.IO;
    using FluentAssertions;
    using Host;
    using Testing;
    using Xunit;

    public class RamlFormatTests
    {
        [Fact]
        public void RegisterSerializer_RegistersContentType()
        {
            const string mediaType = "raml+yaml";
            var appHost = new BasicAppHost();
            appHost.ContentTypes.ContentTypeFormats.ContainsKey(mediaType).Should().BeFalse();

            RamlFormat.RegisterSerializer(appHost);

            appHost.ContentTypes.ContentTypeFormats.ContainsKey(mediaType).Should().BeTrue();
        }

        [Fact]
        public void Serialize_DoesNotUpdateStream_IfDtoNull()
        {
            string result;
            using (var ms = new MemoryStream())
            {
                RamlFormat.Serialize(new BasicRequest(), null, ms);

                var utf8Bytes = ms.ToArray();
                result = utf8Bytes.FromUtf8Bytes();
            }

            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Serialize_ReturnsCorrect()
        {
            string result;
            using (var ms = new MemoryStream())
            {
                RamlFormat.Serialize(new BasicRequest(), new TestObject { Foo = "foo", Bar = 99 }, ms);

                var utf8Bytes = ms.ToArray();
                result = utf8Bytes.FromUtf8Bytes();
            }

            result.Should().Be("foo: foo\r\nbar: 99\r\n");
        }

        [Fact]
        public void Serialize_PrependsRamlVersion_IfSet()
        {
            string result;
            var requestContext = new BasicRequest();
            requestContext.Items.Add("RamlVersion", "#%RAML 0.8");
            using (var ms = new MemoryStream())
            {
                RamlFormat.Serialize(requestContext, new TestObject { Foo = "foo", Bar = 99 }, ms);

                var utf8Bytes = ms.ToArray();
                result = utf8Bytes.FromUtf8Bytes();
            }

            result.Should().Be("#%RAML 0.8\r\nfoo: foo\r\nbar: 99\r\n");
        }
    }

    public class TestObject
    {
        public string Foo { get; set; }
        public int Bar { get; set; }
    }
}
