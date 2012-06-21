using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.Channels.PubSub;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
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
        static SubscriptionSchema subscriptionSchema;

        Establish context = () =>
        {
            address = new Address("TestAddress");
            publisher = new TestDistributor();
            subscriptionChannel = new TestDistributionSubscriber();
            subscriptionSchema = new SubscriptionSchema();
            Configure<IPublisherRegistry>(new PublisherRegistry());
            The<IPublisherRegistry>().RegisterPublisher(address, publisher);

            Configure<ISubscriptionChannelBuilder>(
                new TestSubscriptionChannelBuilder(subscriptionSchema, subscriptionChannel));
            
            request = new MessagePayload(address);
            request.SetSubscriptionRequest(subscriptionSchema);
        };

        Because of = () => Subject.Recieve(request);

        It should_setup_a_subscription_channel_and_subscribe_it_to_the_publisher = () =>
            publisher.Subscribers.ShouldContainOnly(subscriptionChannel);
    }
}