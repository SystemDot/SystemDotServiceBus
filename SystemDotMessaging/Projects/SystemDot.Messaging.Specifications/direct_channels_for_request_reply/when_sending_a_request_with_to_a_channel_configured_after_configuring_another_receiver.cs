using System.Linq;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_with_to_a_channel_configured_after_configuring_another_receiver : WithMessageConfigurationSubject
    {
        const long Message = 1;
        const string Receiver = "Receiver";
        const string Sender = "Sender";

        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel(Receiver).ForRequestReplyReceiving()
                .OpenDirectChannel(Sender).ForRequestReplySendingTo(Receiver)
                .Initialise();

        Because of = () => Bus.SendDirect(Message);

        It should_send_the_message_on_the_sender_channel_with_the_correct_from_address = () =>
            GetServer().SentMessages.Last().GetFromAddress().ShouldBeEquivalentTo(BuildAddress(Sender));
    }
}