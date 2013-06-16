using System;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.receiving.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_on_a_channel : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";

        static Int64 message;
        static MessagePayload payload;
        static TestMessageHandler<Int64> handler;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress)
                    .ForRequestReplySendingTo(ReceiverAddress)
                .Initialise();

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;
            payload = new MessagePayload().MakeSequencedReceivable(
                message, 
                ReceiverAddress, 
                SenderAddress, 
                PersistenceUseType.ReplySend);
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(message);

        It should_send_an_acknowledgement_for_the_message = () =>
            Server.SentMessages.ShouldContain(a => a.GetAcknowledgementId() == payload.GetSourcePersistenceId());
    }
}