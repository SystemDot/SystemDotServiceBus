using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.sending_over_http
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_to_a_receiver_with_a_different_server_but_with_no_addresses_registered 
        : WithHttpConfigurationSubject
    {
        const string ChannelName = "ChannelName";
        const string ReceiverChannelName = "ReceiverChannelName";
        const string ReceiverServerName = "ReceiverServerName";

        static string receiverName;

        Establish context = () =>
        {
            receiverName = ReceiverChannelName + "@" + ReceiverServerName;

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer("ServerName")
                .OpenChannel(ChannelName).ForPointToPointSendingTo(receiverName)
                .Initialise();
        };

        Because of = () => Bus.Send(1);

        It should_send_a_message_with_the_expected_to_address_server_name_set = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetToAddress().Server.Name.ShouldBeEquivalentTo(ReceiverServerName);
    }
}