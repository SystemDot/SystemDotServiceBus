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
    public class when_sending_to_a_receiver_on_a_different_machine_and_remote_proxy : WithConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverChannelName = "TestReceiverChannel";
        const string ReceiverServerName = "TestReceiverServer";
        const string ReceiverServerInstance = "TestReceiverInstance";
        const string RemoteProxyName = "RemoteProxyName";
        const string RemoteProxyInstance = "RemoteProxyInstance";
        static string receiverName;

        
        static TestWebRequestor webRequestor;
        static int message;

        Establish context = () =>
        {
            receiverName = ReceiverChannelName + "@"
                + ReceiverServerName + '/' + ReceiverServerInstance + "." 
                + RemoteProxyName + '/' + RemoteProxyInstance;

            ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

            ConfigureAndRegister<ITaskStarter>();

            webRequestor = new TestWebRequestor(
                new PlatformAgnosticSerialiser(),
                new FixedPortAddress(RemoteProxyName, RemoteProxyInstance));

            ConfigureAndRegister<IWebRequestor>(webRequestor);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer("ServerInstance")
                .OpenChannel(ChannelName).ForPointToPointSendingTo(receiverName)
                .Initialise();

            message = 1;
        };

        Because of = () => Bus.Send(message);

        It should_send_a_message_with_the_correct_to_address_channel_name = () =>
           webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Channel.ShouldEqual(ReceiverChannelName);

        It should_send_a_message_with_the_correct_to_address_server_name = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Server.Name.ShouldEqual(ReceiverServerName);

        It should_send_a_message_with_the_correct_to_address_server_instance = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Server.Instance.ShouldEqual(ReceiverServerInstance);

        It should_send_a_message_with_the_correct_to_address_remote_proxy_name = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Name.ShouldEqual(RemoteProxyName);

        It should_send_a_message_with_the_correct_to_address_remote_proxy_instance = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Instance.ShouldEqual(RemoteProxyInstance);
    }
}