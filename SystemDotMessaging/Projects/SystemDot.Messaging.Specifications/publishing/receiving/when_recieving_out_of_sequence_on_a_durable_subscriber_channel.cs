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
    public class when_recieving_out_of_sequence_on_a_durable_subscriber_channel : 
        WithMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        static int message;
        static MessagePayload payload;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForSubscribingTo(PublisherName)
                    .WithDurability()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;
            payload = new MessagePayload().MakeReceivable(message, PublisherName, ChannelName, PersistenceUseType.SubscriberSend);
            payload.SetFirstSequence(1);
            payload.SetSequenceOriginSetOn(DateTime.Today);
            payload.SetSequence(2);
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_not_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(0);
    }
}