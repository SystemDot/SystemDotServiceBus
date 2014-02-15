using System;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Handling.Actions;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Simple;
using Machine.Specifications;
using FluentAssertions;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.point_to_point
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
        static ActionSubscriptionToken<MessageAddedToCache> addToken;
        static ActionSubscriptionToken<MessageRemovedFromCache> removedToken;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                    .OpenChannel(ChannelName)
                    .ForPointToPointReceiving()
                .Initialise();

            addToken = Messenger.RegisterHandler<MessageAddedToCache>(e => messageAddedToCacheEvent = e);
            removedToken = Messenger.RegisterHandler<MessageRemovedFromCache>(e => messageRemovedFromCacheEvent = e);
            
            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);

            message = 1;

            payload = new MessagePayload().MakeSequencedReceivable(
                message, 
                SenderAddress, 
                ChannelName, 
                PersistenceUseType.PointToPointReceive);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_notify_that_the_message_was_cached = () =>
            messageAddedToCacheEvent.Should().Match<MessageAddedToCache>(m =>
                m.CacheAddress == payload.GetToAddress()
                && m.UseType == PersistenceUseType.PointToPointReceive
                && m.Message == payload);

        It should_push_the_message_to_any_registered_handlers = () => 
            handler.LastHandledMessage.ShouldBeEquivalentTo(message);

        It should_notify_that_the_message_was_removed_from_the_cache = () =>
            messageRemovedFromCacheEvent.Should().Match<MessageRemovedFromCache>(e => 
                e.MessageId == payload.Id
                && e.Address == payload.GetToAddress()
                && e.UseType == PersistenceUseType.PointToPointReceive);

        It should_send_an_acknowledgement_for_the_message = () =>
            GetServer().SentMessages.Should().Contain(a => a.GetAcknowledgementId() == payload.GetSourcePersistenceId());
    }
    
   
}