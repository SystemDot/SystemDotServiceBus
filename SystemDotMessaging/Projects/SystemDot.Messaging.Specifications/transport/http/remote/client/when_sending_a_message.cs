using System;
using System.Linq;
using SystemDot.Http;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.configuration;
using SystemDot.Serialisation;
using Machine.Specifications;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message : WithConfigurationSubject
    {
        static IBus bus;
        static TestWebRequestor webRequestor;
        static int message;
       
        Establish context = () =>
        {
            webRequestor = new TestWebRequestor(
                new PlatformAgnosticSerialiser(), 
                new FixedPortAddress(Environment.MachineName));

            ConfigureAndRegister<IWebRequestor>(webRequestor);

            bus = Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteClientOf(MessageServer.Local())
                .OpenChannel("TestSender")
                .ForPointToPointSendingTo("TestReceiver")
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_serialise_the_message_and_send_it_as_a_put_request_to_the_message_server = () =>
            webRequestor.RequestsMade.Single()
                .Deserialise<MessagePayload>(Resolve<ISerialiser>())
                    .DeserialiseTo<int>()
                        .ShouldEqual(message);
    }
}
