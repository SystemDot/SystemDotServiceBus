using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Local;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.Specifications.distribution;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.publishing
{
    public class when_handling_a_subscribe_request_message 
        : WithDistributionSubscriberSubject<SubsriptionRequestHandler>
    {
        static Address address;
        static TestDistributor publisher;
        static IDistributionSubscriber subscriptionChannel;
        static MessagePayload request;
 
        Establish context = () =>
        {
            address = new Address("TestAddress");
            publisher = new TestDistributor();
            subscriptionChannel = new TestDistributionSubscriber();

            Configure<PublisherRegistry>(new PublisherRegistry());
            The<PublisherRegistry>().RegisterPublisher(address, publisher);

            Configure<IDistributionSubscriber>(subscriptionChannel);
            
            request = new MessagePayload(address);
        };

        Because of = () => Subject.Recieve(request);

        It should_setup_a_subscription_channel_and_subscribe_it_to_the_publisher = () =>
            publisher.Subscribers.ShouldContainOnly(subscriptionChannel);
    }
}