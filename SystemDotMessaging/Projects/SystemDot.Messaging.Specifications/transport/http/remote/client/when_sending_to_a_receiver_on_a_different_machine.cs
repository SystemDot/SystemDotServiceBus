using System;
using System.Linq;
using SystemDot.Http;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Specifications.channels;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_to_a_receiver_on_a_different_machine : WithConfigurationSubject
    {
        const string ProxyInstance = "RemoteProxyInstance";
        const string SenderAddress = "Test";
        const string ReceiverChannelName = "TestReceiverChannel";
        const string ReceiverServerName = "TestReceiverServer";
        const string ReceiverServerInstance = "TestReceiverInstance";
        const string ReceiverAddress = ReceiverChannelName + "@" + ReceiverServerName + "/" + ReceiverServerInstance;
        
        
        static TestWebRequestor webRequestor;
        static int message;
        
        Establish context = () =>
        {
            ConfigureAndRegister<ITaskStarter>();

            webRequestor = new TestWebRequestor(
                new PlatformAgnosticSerialiser(),
                new FixedPortAddress(ReceiverServerName, ReceiverServerInstance));

            ConfigureAndRegister<IWebRequestor>(webRequestor);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteClient("RemoteClientInstance")
                .UsingProxy(MessageServer.Local(ProxyInstance))
                .OpenChannel(SenderAddress).ForPointToPointSendingTo(ReceiverAddress)
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

        It should_send_a_message_with_the_to_address_remote_proxy_name_set_to_the_local_machine = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Name.ShouldEqual(ReceiverServerName);

        It should_send_a_message_with_the_to_address_remote_proxy_instance_set_correctly = () =>
            webRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Instance.ShouldEqual(ReceiverServerInstance);
    }
}