using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.sending_over_http
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_to_a_receiver_on_a_different_server : WithHttpConfigurationSubject
    {
        const string ChannelName = "ChannelName";
        const string ReceiverChannelName = "ReceiverChannelName";
        const string ReceiverServerName = "ReceiverServerName";
        const string ReceiverServerAddress = "ReceiverServerAddress";
        const string ReceiverName = ReceiverChannelName + "@" + ReceiverServerName;

        Establish context = () =>
        {
            ServerAddressConfiguration.AddAddress(ReceiverServerName, ReceiverServerAddress);
            ServerAddressConfiguration.AddAddress("OtherName", "OtherAddress");

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
                .GetToAddress().Server.Name.ShouldEqual(ReceiverServerName);

        It should_send_a_message_with_the_correct_to_address_server_address = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Server.Address.Path.ShouldEqual(ReceiverServerAddress);
    }
}