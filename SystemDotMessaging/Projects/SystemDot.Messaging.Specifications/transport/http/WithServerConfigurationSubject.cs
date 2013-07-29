using System.Collections.Generic;
using System.IO;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http
{
    public class WithServerConfigurationSubject : WithHttpConfigurationSubject
    {
        Establish context = () => ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

        Cleanup cleanup = () => TestHttpServer.ClearInstance();

        public static IEnumerable<MessagePayload> SendMessagesToServer(params MessagePayload[] toSend)
        {
            return SendObjectsToServer(toSend).Deserialise<IEnumerable<MessagePayload>>(new JsonSerialiser());
        }

        public static Stream SendObjectsToServer(params object[] toSend)
        {
            Stream request = new MemoryStream();
            var serialiser = new JsonSerialiser();

            toSend.ForEach(m => request.Serialise(m, serialiser));

            return TestHttpServer.Instance.Request(request);
        }
    }
}