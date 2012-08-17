using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.publishing
{
    [Subject("Message publishing")]
    public class when_handling_a_subscribe_request_message 
        : WithMessageInputterSubject<SubscriptionRequestHandler>
    {
        static EndpointAddress address;
        static TestDistributor publisher;
        static IMessageInputter<MessagePayload> subscriptionChannel;
        static MessagePayload request;
        static SubscriptionSchema subscriptionSchema;

        Establish context = () =>
        {
            address = new EndpointAddress("TestAddress", "TestServer");
            
            publisher = new TestDistributor();
            Configure<IPublisherRegistry>(new PublisherRegistry());
            The<IPublisherRegistry>().RegisterPublisher(address, publisher);

            subscriptionSchema = new SubscriptionSchema(new EndpointAddress("TestSubscriberAddress", "TestServer"));
            subscriptionChannel = new Pipe<MessagePayload>();
            Configure<IChannelBuilder>(new TestChannelBuilder(address, subscriptionSchema.SubscriberAddress, subscriptionChannel));
            
            request = new MessagePayload();
            request.SetToAddress(address);
            request.SetSubscriptionRequest(subscriptionSchema);
        };

        Because of = () => Subject.InputMessage(request);

        It should_setup_a_subscription_channel_and_subscribe_it_to_the_publisher = () =>
            publisher.Subscribers.ShouldContainOnly(subscriptionChannel);
    }
}