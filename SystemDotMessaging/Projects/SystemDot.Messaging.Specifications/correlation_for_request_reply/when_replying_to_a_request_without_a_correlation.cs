using System;
using System.Linq;
using SystemDot.Messaging.Correlation;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.correlation_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_to_a_request_without_a_correlation : WithMessageConfigurationSubject
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
                .Sequenced();

            GetServer().ReceiveMessage(messagePayload);
        };

        Because of = () => Bus.Reply(1);

        It should_reply_with_a_message_without_any_correlation = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().HasCorrelationId().ShouldBeFalse();
    }
}