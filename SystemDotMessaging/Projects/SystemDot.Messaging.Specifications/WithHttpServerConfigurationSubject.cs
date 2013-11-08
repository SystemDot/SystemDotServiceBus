using System.Collections.Generic;
using System.IO;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications
{
    public class WithHttpServerConfigurationSubject : WithHttpConfigurationSubject
    {
        Establish context = () => ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

        Cleanup cleanup = () => TestHttpServer.ClearInstance();

        public static IEnumerable<MessagePayload> SendMessagesToServer(MessagePayload toSend)
        {
            return SendObjectsToServer(toSend).Deserialise<IEnumerable<MessagePayload>>(new JsonSerialiser());
        }

        public static Stream SendObjectsToServer(object toSend)
        {
            Stream request = new MemoryStream();
            var serialiser = new JsonSerialiser();

            request.Serialise(toSend, serialiser);

            return TestHttpServer.Instance.Request(request);
        }

        protected new static void ReInitialise()
        {
            TestHttpServer.ClearInstance();
            WithHttpConfigurationSubject.ReInitialise();
        }
    }
}