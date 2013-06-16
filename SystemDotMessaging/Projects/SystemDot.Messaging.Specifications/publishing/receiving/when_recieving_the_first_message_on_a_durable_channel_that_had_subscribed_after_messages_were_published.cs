using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.publishing.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_recieving_the_first_message_on_a_durable_channel_that_had_subscribed_after_messages_were_published 
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        static MessagePayload payload;
        static TestMessageHandler<Int64> handler;
        static int message;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForSubscribingTo(PublisherName)
                .WithDurability()
                .Initialise();

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;
            payload = new MessagePayload().MakeSequencedReceivable(message, PublisherName, ChannelName, PersistenceUseType.SubscriberSend);
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(message);    
    }
}