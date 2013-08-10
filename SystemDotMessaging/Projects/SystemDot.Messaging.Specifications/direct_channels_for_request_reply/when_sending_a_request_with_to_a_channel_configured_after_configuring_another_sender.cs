using System.Linq;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_with_to_a_channel_configured_after_configuring_another_sender : WithMessageConfigurationSubject
    {
        const long Message = 1;
        const string OtherSender = "OtherSender";

        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel("Sender").ForRequestReplySendingTo("Receiver")
                .OpenDirectChannel(OtherSender).ForRequestReplySendingTo("OtherReceiver")
                .Initialise();

        Because of = () => Bus.Send(Message);

        It should_send_the_message_on_the_second_channel_with_the_correct_from_address = () => 
            GetServer().SentMessages.Last().GetFromAddress().ShouldEqual(BuildAddress(OtherSender));
    }
}