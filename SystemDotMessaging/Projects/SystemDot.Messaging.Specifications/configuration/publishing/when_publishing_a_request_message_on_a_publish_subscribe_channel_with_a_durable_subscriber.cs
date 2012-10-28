using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")]
    public class when_publishing_a_request_message_on_a_publish_subscribe_channel_with_a_durable_subscriber
        : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName = "TestSubscriber";

        static IBus bus;
        static int message;
        
        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            message = 1;
            SubscribeDurable(BuildAddress(SubscriberName), BuildAddress(ChannelName));
        };

        Because of = () => bus.Publish(message);

        It should_have_persisted_the_message = () =>
            Resolve<IDatastore>()
                .GetMessages(PersistenceUseType.SubscriberSend, BuildAddress(SubscriberName))
                .ShouldNotBeEmpty();
    }
}