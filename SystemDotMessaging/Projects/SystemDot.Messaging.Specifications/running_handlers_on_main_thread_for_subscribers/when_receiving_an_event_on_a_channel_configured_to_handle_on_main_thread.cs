using SystemDot.Messaging.Packaging;
using FluentAssertions;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.running_handlers_on_main_thread_for_subscribers
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_event_on_a_channel_configured_to_handle_on_main_thread : WithMessageConfigurationSubject
    {
        const string SubscriberChannel = "SubscriberChannel";
        const string PublisherChannel = "PublisherChannel";

        static MessagePayload payload;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SubscriberChannel)
                    .ForSubscribingTo(PublisherChannel)
                    .HandleEventsOnMainThread()
                .Initialise();

            payload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(PublisherChannel)
                .SetToChannel(SubscriberChannel)
                .SetChannelType(PersistenceUseType.SubscriberSend)
                .Sequenced();
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_switch_to_the_main_thread_to_handle_the_message = () => MainThreadMarshaller.WasRunThrough.Should().BeTrue();
    }
}