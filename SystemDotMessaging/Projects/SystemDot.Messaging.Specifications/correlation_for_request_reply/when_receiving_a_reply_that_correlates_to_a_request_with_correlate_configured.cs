using System;
using System.Linq;
using SystemDot.Messaging.Correlation;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.correlation_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_that_correlates_to_a_request_with_correlate_configured :
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
            
            messagePayload.SetCorrelationId(GetServer().SentMessages.First().GetCorrelationId());
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload);

        It should_handle_the_request = () => handler.LastHandledMessage.ShouldEqual(Message);
    }
}