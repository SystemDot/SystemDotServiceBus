using System;
using System.Linq;
using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Specifications.channels;
using SystemDot.Serialisation;
using Machine.Specifications;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.Specifications.transport.http
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message : WithConfigurationSubject
    {
        const string ServerInstance = "ServerInstance";

        static IBus bus;
        static TestWebRequestor webRequestor;
        static int message;
       
        Establish context = () =>
        {
            ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

            webRequestor = new TestWebRequestor(
                new PlatformAgnosticSerialiser(), 
                new FixedPortAddress(Environment.MachineName, ServerInstance));

            ConfigureAndRegister<IWebRequestor>(webRequestor);

            bus = Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ServerInstance)
                .OpenChannel("TestSender")
                .ForPointToPointSendingTo("TestReceiver")
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_serialise_the_message_and_send_it_as_a_put_request_to_the_message_server = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .DeserialiseTo<int>().ShouldEqual(message);

        It should_send_a_message_with_the_correct_to_address_server_name = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Server.Name.ShouldEqual(Environment.MachineName);

        It should_send_a_message_with_the_correct_to_address_server_instance = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Server.Instance.ShouldEqual(ServerInstance);
    }
}
