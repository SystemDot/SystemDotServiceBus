using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_on_a_channel_with_another_setup : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test1";
        
        

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForRequestReplyReceiving()
                .OpenChannel("Test2").ForRequestReplyReceiving()
                .Initialise();

            GetServer().ReceiveMessage(
                new MessagePayload()
                    .SetMessageBody(1)
                    .SetFromChannel("TestSender")
                    .SetToChannel(ChannelName)
                    .SetChannelType(PersistenceUseType.RequestSend)
                    .Sequenced());
        };

        Because of = () => Bus.Reply(1);

        It should_only_send_the_message_to_the_correct_channel = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().Count.ShouldBeEquivalentTo(1);
    }
}