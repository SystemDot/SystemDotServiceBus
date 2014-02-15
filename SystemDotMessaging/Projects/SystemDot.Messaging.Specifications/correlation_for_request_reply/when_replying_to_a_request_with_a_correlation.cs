using System.Linq;
using SystemDot.Messaging.Correlation;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.correlation_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_to_a_request_with_a_correlation : WithMessageConfigurationSubject
    {
        const string ReceiverChannel = "ReceiverChannel";

        static MessagePayload messagePayload;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverChannel)
                .ForRequestReplyReceiving()
                .Initialise();

            messagePayload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel("SenderChannel")
                .SetToChannel(ReceiverChannel)
                .SetChannelType(PersistenceUseType.RequestSend)
                .Sequenced()
                .SetNewCorrelationId();
                
            GetServer().ReceiveMessage(messagePayload);
        };

        Because of = () => Bus.Reply(1);

        It should_reply_with_a_message_with_the_same_correlation_as_the_request = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetCorrelationId().ShouldBeEquivalentTo(messagePayload.GetCorrelationId());
    }
}