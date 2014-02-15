using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.hooks_for_publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_event_on_a_subscriber_channel_with_a_pre_unpackaging_hook : WithPublisherSubject
    {
        const string SubscriberAddress = "SubscriberAddress";
        const string PublisherAddress = "PublisherAddress";
        const Int64 Message = 1;
        
        static MessagePayload payload;
        static TestMessagePayloadProcessorHook hook;

        Establish context = () =>
        {
            hook = new TestMessagePayloadProcessorHook();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SubscriberAddress)
                    .ForSubscribingTo(PublisherAddress)
                    .WithHook(hook)
                .Initialise();

            payload = new MessagePayload().MakeSequencedReceivable(
                Message,
                PublisherAddress,
                SubscriberAddress,
                PersistenceUseType.SubscriberSend);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_run_the_message_through_the_hook = () => hook.Message.Should().BeSameAs(payload);
    }
}