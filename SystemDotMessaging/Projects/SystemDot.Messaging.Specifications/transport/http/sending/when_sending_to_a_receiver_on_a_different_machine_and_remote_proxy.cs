using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_to_a_receiver_on_a_different_machine_and_remote_proxy : WithHttpConfigurationSubject
    {
        const string ChannelName = "ChannelName";
        const string ReceiverChannelName = "ReceiverChannelName";
        const string ReceiverServerName = "ReceiverServerName";
        const string RemoteProxyName = "RemoteProxyName";
        const string RemoteProxyAddress = "RemoteProxyAddress";
        const string ReceiverName = ReceiverChannelName + "@" + ReceiverServerName + "." + RemoteProxyName;

        Establish context = () =>
        {
            ServerAddressConfiguration.AddAddress(RemoteProxyName, RemoteProxyAddress);
            ServerAddressConfiguration.AddAddress("OtherName", "OtherAddress");

            WebRequestor.ExpectAddress(RemoteProxyName, RemoteProxyAddress);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer("ServerName")
                .OpenChannel(ChannelName).ForPointToPointSendingTo(ReceiverName)
                .Initialise();
        };

        Because of = () => Bus.Send(1);

        It should_send_a_message_with_the_correct_to_address_channel_name = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Channel.ShouldEqual(ReceiverChannelName);

        It should_send_a_message_with_the_correct_to_address_server_name = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Server.Name.ShouldEqual(ReceiverServerName);

        It should_send_a_message_with_the_correct_to_address_remote_proxy_name = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Name.ShouldEqual(RemoteProxyName);
    }
}