using System;
using System.Linq;
using SystemDot.Http;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Specifications.channels;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Specifications;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_using_a_local_proxy : WithConfigurationSubject
    {
        const string ClientInstance = "ClientInstance";
        const string ProxyInstance = "ProxyInstance";
        const string ChannelAddress = "TestSender";
        const string RecieverAddress = "TestReceiver";

        
        static TestWebRequestor webRequestor;
        static int message;

        Establish context = () =>
        {
            ConfigureAndRegister<ITaskStarter>();

            webRequestor = new TestWebRequestor(
                new PlatformAgnosticSerialiser(),
                new FixedPortAddress(Environment.MachineName, ProxyInstance));

            ConfigureAndRegister<IWebRequestor>(webRequestor);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteClient(ClientInstance)
                .UsingProxy(MessageServer.Local(ProxyInstance))
                .OpenChannel(ChannelAddress)
                .ForPointToPointSendingTo(RecieverAddress)
                .Initialise();

            message = 1;
        };

        Because of = () => Bus.Send(message);

        It should_serialise_the_message_and_send_it_as_a_put_request_to_the_message_server = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .DeserialiseTo<int>().ShouldEqual(message);

        It should_send_a_message_with_the_to_address_channel_name_set_correctly = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Channel.ShouldEqual(RecieverAddress);

        It should_send_a_message_with_the_to_address_server_set_to_local = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Server.Name.ShouldEqual(Environment.MachineName);

        It should_send_a_message_with_the_to_address_server_instance_set_correctly = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Server.Instance.ShouldEqual(ClientInstance);

        It should_send_a_message_with_the_to_address_proxy_set_to_the_local_machine = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Name.ShouldEqual(Environment.MachineName);

        It should_send_a_message_with_the_to_address_proxy_instance_set_correctly = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Instance.ShouldEqual(ProxyInstance);

        It should_send_a_message_with_the_from_address_channel_name_set_correctly = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().Channel.ShouldEqual(ChannelAddress);

        It should_send_a_message_with_the_from_address_server_set_to_local = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().ServerPath.Server.Name.ShouldEqual(Environment.MachineName);

        It should_send_a_message_with_the_from_address_server_instance_set_correctly = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().ServerPath.Server.Instance.ShouldEqual(ClientInstance);

        It should_send_a_message_with_the_from_address_proxy_set_to_the_local_machine = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().ServerPath.Proxy.Name.ShouldEqual(Environment.MachineName);

        It should_send_a_message_with_the_from_address_proxy_instance_set_correctly = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().ServerPath.Proxy.Instance.ShouldEqual(ProxyInstance);
    }
}
