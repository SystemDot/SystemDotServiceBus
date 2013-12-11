using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing;

namespace SystemDot.Messaging.Specifications
{
    public class WithPublisherSubject : WithMessageConfigurationSubject
    {
        protected static void Subscribe(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            GetServer().ReceiveMessage(CreateSubscriptionRequest(subscriberAddress, publisherAddress));
        }

        protected static void SubscribeDurable(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            MessagePayload request = CreateSubscriptionRequest(subscriberAddress, publisherAddress);
            request.GetSubscriptionRequestSchema().IsDurable = true;
            GetServer().ReceiveMessage(request);
        }

        static MessagePayload CreateSubscriptionRequest(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            return new MessagePayload()
                .SetFromEndpointAddress(subscriberAddress)
                .SetToEndpointAddress(publisherAddress)
                .AsSubscriptionRequest();
        }
    }
}