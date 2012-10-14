using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")]
    public class when_publishing_a_request_message_on_a_durable_publish_subscribe_channel
        : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName = "TestSubscriber";

        static IBus bus;
        static int message;
        static TestPersistence persistence;

        Establish context = () =>
        {
            var persistenceFactory = new TestPersistenceFactory();
            ConfigureAndRegister<IPersistenceFactory>(persistenceFactory);

            persistence = new TestPersistence();
            persistenceFactory.AddPersistence(PersistenceUseType.PublisherSend, persistence);
            persistenceFactory.AddPersistence(PersistenceUseType.SubscriberSend, new TestPersistence());

            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing().WithDurability()
                .Initialise();

            message = 1;
            Subscribe(BuildAddress(SubscriberName), BuildAddress(ChannelName));
        };

        Because of = () => bus.Publish(message);

        It should_have_persisted_the_message = () => persistence.LastAddedMessage.ShouldNotBeNull();

        It should_have_decached_the_message_after_successful_publishing = () => persistence.GetMessages().ShouldBeEmpty();
    }
}