using System.Linq;
using FluentAssertions;
using Machine.Specifications;using FluentAssertions;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Direct;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request : WithMessageConfigurationSubject
    {
        const long Message = 1;
        const string Sender = "Sender";
        const string Receiver = "Receiver";

        Establish context = () => 
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel(Sender).ForRequestReplySendingTo(Receiver)
                .Initialise();

        Because of = () => Bus.SendDirect(Message);

        It should_send_the_message_with_the_correct_payload =
            () => GetServer().SentMessages.Single().DeserialiseTo<long>().ShouldBeEquivalentTo(Message);

        It should_send_the_message_with_the_correct_to_address =
            () => GetServer().SentMessages.Single().GetToAddress().ShouldBeEquivalentTo(BuildAddress(Receiver));

        It should_send_the_message_with_the_correct_from_address =
            () => GetServer().SentMessages.Single().GetFromAddress().ShouldBeEquivalentTo(BuildAddress(Sender));

        It should_send_the_message_marked_for_direct_channeling =
            () => GetServer().SentMessages.Single().IsDirectChannelMessage().Should().BeTrue();
    }
}