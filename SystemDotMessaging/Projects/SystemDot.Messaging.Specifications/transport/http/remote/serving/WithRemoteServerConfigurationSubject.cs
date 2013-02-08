using System.Collections.Generic;
using System.IO;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.configuration;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications.transport.http.remote.serving
{
    public class WithRemoteServerConfigurationSubject : WithMessageConfigurationSubject
    {
        public static IEnumerable<MessagePayload> SendMessagesToRemoteServer(params MessagePayload[] toSend)
        {
            return SendObjectsToRemoteServer(toSend)
                .Deserialise<IEnumerable<MessagePayload>>(new PlatformAgnosticSerialiser());
        }

        public static Stream SendObjectsToRemoteServer(params object[] toSend)
        {
            Stream request = new MemoryStream();
            var serialiser = new PlatformAgnosticSerialiser();

            toSend.ForEach(m => request.Serialise(m, serialiser));

            return TestHttpServer.Instance.Request(request);
        }
    }
}