using System.Collections.Generic;
using System.IO;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.channels;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications.transport.http
{
    public class WithServerConfigurationSubject : WithMessageConfigurationSubject
    {
        public static IEnumerable<MessagePayload> SendMessagesToServer(params MessagePayload[] toSend)
        {
            return SendObjectsToServer(toSend)
                .Deserialise<IEnumerable<MessagePayload>>(new PlatformAgnosticSerialiser());
        }

        public static Stream SendObjectsToServer(params object[] toSend)
        {
            Stream request = new MemoryStream();
            var serialiser = new PlatformAgnosticSerialiser();

            toSend.ForEach(m => request.Serialise(m, serialiser));

            return TestHttpServer.Instance.Request(request);
        }
    }
}