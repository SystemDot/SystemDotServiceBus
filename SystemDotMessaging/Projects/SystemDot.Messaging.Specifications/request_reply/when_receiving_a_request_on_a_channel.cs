using System;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.RequestReply.Builders;
using SystemDot.Messaging.Simple;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_on_a_channel : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";
        const Int64 Message = 1;

        static MessagePayload payload;
        static TestMessageHandler<Int64> handler;
        
        static RequestReceiveChannelBuilt requestReceiveChannelBuiltEvent;
        static ReplySendChannelBuilt replySendChannelBuiltEvent;

        Establish context = () =>
        {
             Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                    .OpenChannel(ReceiverAddress)
                    .ForRequestReplyReceiving()
                .Initialise();

            Messenger.RegisterHandler<ReplySendChannelBuilt>(m => replySendChannelBuiltEvent = m);
            Messenger.RegisterHandler<RequestReceiveChannelBuilt>(m => requestReceiveChannelBuiltEvent = m);

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);

            payload = new MessagePayload().MakeSequencedReceivable(
                Message, 
                SenderAddress, 
                ReceiverAddress, 
                PersistenceUseType.RequestSend);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(Message);

        It should_notify_that_the_request_receive_channel_was_built = () => requestReceiveChannelBuiltEvent
            .ShouldMatch(m => 
                m.CacheAddress == BuildAddress(SenderAddress)
                && m.SenderAddress == BuildAddress(SenderAddress)
                && m.ReceiverAddress == BuildAddress(ReceiverAddress));

        It should_notify_that_the_reply_send_channel_was_built = () => replySendChannelBuiltEvent
            .ShouldMatch(m =>
                m.CacheAddress == BuildAddress(SenderAddress)
                && m.ReceiverAddress == BuildAddress(ReceiverAddress)
                && m.SenderAddress == BuildAddress(SenderAddress));

        It should_send_an_acknowledgement_for_the_message = () =>
            GetServer().SentMessages.ShouldContain(a => a.GetAcknowledgementId() == payload.GetSourcePersistenceId());
    }
}