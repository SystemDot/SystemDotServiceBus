using System.Linq;
using SystemDot.Messaging.Correlation;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.correlation_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_two_replies_that_correlate_with_two_requests_with_correlate_configured :
        WithMessageConfigurationSubject
    {
        const string RecieverAddress = "ReceiverChannel";
        const string SenderChannel = "SenderChannel";
        const long Message1 = 1;
        const long Message2 = 2;

        static TestMessageHandler<long> handler;
        static MessagePayload messagePayload1;
        static MessagePayload messagePayload2;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderChannel)
                .ForRequestReplySendingTo(RecieverAddress).CorrelateReplyToRequest()
                .RegisterHandlers(h => h.RegisterHandler(handler))
                .Initialise();

            Bus.Send(Message1);

            messagePayload1 = new MessagePayload()
                .SetMessageBody(Message1)
                .SetFromChannel(RecieverAddress)
                .SetToChannel(SenderChannel)
                .SetChannelType(PersistenceUseType.ReplySend)
                .Sequenced();

            messagePayload1.SetCorrelationId(GetServer().SentMessages.First().GetCorrelationId());

            Bus.Send(Message2);

            messagePayload2 = new MessagePayload()
                .SetMessageBody(Message2)
                .SetFromChannel(RecieverAddress)
                .SetToChannel(SenderChannel)
                .SetChannelType(PersistenceUseType.ReplySend)
                .Sequenced();

            messagePayload2.SetCorrelationId(GetServer().SentMessages.Last().GetCorrelationId());
        };

        Because of = () =>
        {
            GetServer().ReceiveMessage(messagePayload1);
            GetServer().ReceiveMessage(messagePayload2);
        };

        It should_handle_the_requests = () => handler.HandledMessages.Count.ShouldBeEquivalentTo(2);
    }
}