using System;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_on_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";

        static Int64 message;
        static MessagePayload payload;
        static TestMessageHandler<Int64> handler;
        static MessageAddedToCache messageAddedToCacheEvent;
        static MessageRemovedFromCache messageRemovedFromCacheEvent;

        Establish context = () =>
        {
            Messenger.Register<MessageAddedToCache>(e => messageAddedToCacheEvent = e);
            Messenger.Register<MessageRemovedFromCache>(e => messageRemovedFromCacheEvent = e);
            
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                    .OpenChannel(ChannelName)
                    .ForPointToPointReceiving()
                .Initialise();

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;

            payload = new MessagePayload().MakeSequencedReceivable(
                message, 
                SenderAddress, 
                ChannelName, 
                PersistenceUseType.PointToPointReceive);
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_notify_that_the_message_was_cached = () =>
            messageAddedToCacheEvent.ShouldMatch(m =>
                m.CacheAddress == payload.GetToAddress()
                && m.UseType == PersistenceUseType.PointToPointReceive
                && m.Message == payload);

        It should_push_the_message_to_any_registered_handlers = () => 
            handler.LastHandledMessage.ShouldEqual(message);

        It should_notify_that_the_message_was_removed_from_the_cache = () =>
            messageRemovedFromCacheEvent.ShouldMatch(e => 
                e.MessageId == payload.Id
                && e.Address == payload.GetToAddress()
                && e.UseType == PersistenceUseType.PointToPointReceive);

        It should_send_an_acknowledgement_for_the_message = () =>
            Server.SentMessages.ShouldContain(a => a.GetAcknowledgementId() == payload.GetSourcePersistenceId());
    }
    
   
}