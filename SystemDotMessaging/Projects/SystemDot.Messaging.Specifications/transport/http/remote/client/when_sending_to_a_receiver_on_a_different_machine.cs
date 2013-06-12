using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_to_a_receiver_on_a_different_machine : WithHttpConfigurationSubject
    {
        const string Proxy = "Proxy";
        const string SenderAddress = "SenderAddress";
        const string ReceiverChannelName = "ReceiverChannelName";
        const string ReceiverServerName = "ReceiverServerName";
        const string ReceiverServerAddress = "ReceiverServerAddress";
        const string ReceiverAddress = ReceiverChannelName + "@" + ReceiverServerName;
        
        Establish context = () =>
        {
            ConfigureAndRegister<ITaskStarter>();

            ServerAddressConfiguration.AddAddress(ReceiverServerName, ReceiverServerAddress);
            WebRequestor.ExpectAddress(ReceiverServerName, ReceiverServerAddress);
            
            Messaging.Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServerUsingAProxy("Server", Proxy)
                .OpenChannel(SenderAddress).ForPointToPointSendingTo(ReceiverAddress)
                .Initialise();
        };

        Because of = () => Bus.Send(1);

        It should_send_a_message_with_the_correct_to_address_channel_name = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Channel.ShouldEqual(ReceiverChannelName);

        It should_send_a_message_with_the_correct_to_address_server_name = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Server.Name.ShouldEqual(ReceiverServerName);

        It should_send_a_message_with_the_to_address_remote_proxy_name_set_to_the_local_machine = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Name.ShouldEqual(ReceiverServerName);
    }
}