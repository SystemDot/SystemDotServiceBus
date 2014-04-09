using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.correlation_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_that_has_no_correlation_with_correlate_configured :
        WithMessageConfigurationSubject
    {
        const string RecieverAddress = "ReceiverChannel";
        const string SenderChannel = "SenderChannel";
        const long Message = 1;

        static TestMessageHandler<long> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderChannel)
                .ForRequestReplySendingTo(RecieverAddress).CorrelateReplyToRequest()
                .RegisterHandlers(h => h.RegisterHandler(handler))
                .Initialise();

            Bus.Send(Message);

            messagePayload = new MessagePayload()
                .SetMessageBody(Message)
                .SetFromChannel(RecieverAddress)
                .SetToChannel(SenderChannel)
                .SetChannelType(PersistenceUseType.ReplySend)
                .Sequenced();
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload);

        It should_handle_the_request = () => handler.LastHandledMessage.ShouldBeEquivalentTo(Message);
    }
}