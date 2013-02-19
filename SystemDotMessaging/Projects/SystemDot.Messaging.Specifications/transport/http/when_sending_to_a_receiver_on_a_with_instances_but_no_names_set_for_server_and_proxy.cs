using System;
using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_to_a_receiver_on_a_with_instances_but_no_names_set_for_server_and_proxy : WithConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverChannelName = "TestReceiverChannel";
        const string ReceiverServerInstance = "TestReceiverInstance";
        const string RemoteProxyInstance = "RemoteProxyInstance";
        static string receiverName;

        static IBus bus;
        static TestWebRequestor webRequestor;
        static int message;

        Establish context = () =>
        {
            receiverName = ReceiverChannelName + "@" + '/' + ReceiverServerInstance + "." + '/' + RemoteProxyInstance;

            ConfigureAndRegister<ITaskStarter>();
            ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

            webRequestor = new TestWebRequestor(
                new PlatformAgnosticSerialiser(),
                new FixedPortAddress(Environment.MachineName, RemoteProxyInstance));

            ConfigureAndRegister<IWebRequestor>(webRequestor);

            bus = Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer("ServerInstance")
                .OpenChannel(ChannelName).ForPointToPointSendingTo(receiverName)
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_send_a_message_with_the_to_address_server_name_set_to_the_local_machine = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Server.Name.ShouldEqual(Environment.MachineName);

        It should_send_a_message_with_the_to_address_remote_proxy_name_set_to_the_local_machine = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Name.ShouldEqual(Environment.MachineName);
    }
}