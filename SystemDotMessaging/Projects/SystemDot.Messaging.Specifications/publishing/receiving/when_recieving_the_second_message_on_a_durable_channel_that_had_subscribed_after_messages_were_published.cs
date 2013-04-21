using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.publishing.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_recieving_the_second_message_on_a_durable_channel_that_had_subscribed_after_messages_were_published : WithMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        static MessagePayload payload;
        static TestMessageHandler<int> handler;
        static int message;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForSubscribingTo(PublisherName)
                .WithDurability()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            payload = new MessagePayload().MakeReceivable(1, PublisherName, ChannelName, PersistenceUseType.SubscriberSend);
            payload.SetFirstSequence(2);
            payload.SetSequenceOriginSetOn(DateTime.Today);
            payload.SetSequence(2);

            Server.ReceiveMessage(payload);

            message = 2;
            payload = new MessagePayload().MakeReceivable(message, PublisherName, ChannelName, PersistenceUseType.SubscriberSend);
            payload.SetFirstSequence(2);
            payload.SetSequenceOriginSetOn(DateTime.Today);
            payload.SetSequence(3);
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(message);
    }
}