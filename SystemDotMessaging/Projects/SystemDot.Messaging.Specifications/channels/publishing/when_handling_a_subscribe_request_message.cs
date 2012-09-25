using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Publishing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_handling_a_subscription_channel : WithMessageInputterSubject<SubscriptionRequestHandler>
    {
        static EndpointAddress address;
        static TestPublisher publisher;
        static MessagePayload request;
        static SubscriptionSchema subscriptionSchema;

        Establish context = () =>
        {
            address = new EndpointAddress("TestAddress", "TestServer");
            
            publisher = new TestPublisher();
            Configure<IPublisherRegistry>(new PublisherRegistry());
            The<IPublisherRegistry>().RegisterPublisher(address, publisher);

            subscriptionSchema = new SubscriptionSchema(new EndpointAddress("TestSubscriberAddress", "TestServer"));
            
            request = new MessagePayload();
            request.SetToAddress(address);
            request.SetSubscriptionRequest(subscriptionSchema);
        };

        Because of = () => Subject.InputMessage(request);

        It should_setup_a_subscription_channel_and_subscribe_it_to_the_publisher = () => publisher.Subscribers.ShouldNotBeEmpty();
    }
}