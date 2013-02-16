using SystemDot.Http;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_using_a_remote_proxy : WithConfigurationSubject
    {
        const string ClientInstance = "ClientInstance";
        const string ProxyName = "ProxyName";
        const string ProxyInstance = "ProxyInstance";
        const string ChannelAddress = "TestSender";
        const string RecieverAddress = "TestReceiver";

        static IBus bus;
        static TestWebRequestor webRequestor;
        static int message;

        Establish context = () =>
        {
            ConfigureAndRegister<ITaskStarter>();

            webRequestor = new TestWebRequestor(
                new PlatformAgnosticSerialiser(),
                new FixedPortAddress(ProxyName, ProxyInstance));

            ConfigureAndRegister<IWebRequestor>(webRequestor);

            bus = Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteClient(ClientInstance)
                .UsingProxy(MessageServer.Named(ProxyName, ProxyInstance))
                .OpenChannel(ChannelAddress)
                .ForPointToPointSendingTo(RecieverAddress)
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);
        
        It should_send_a_message_with_the_to_address_proxy_set_to_the_specified_remote = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Name.ShouldEqual(ProxyName);

        It should_send_a_message_with_the_to_address_proxy_instance_set_correctly = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Instance.ShouldEqual(ProxyInstance);

        It should_send_a_message_with_the_from_address_proxy_set_to_the_specified_remote = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().ServerPath.Proxy.Name.ShouldEqual(ProxyName);

        It should_send_a_message_with_the_from_address_proxy_instance_set_correctly = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().ServerPath.Proxy.Instance.ShouldEqual(ProxyInstance);
    }
}