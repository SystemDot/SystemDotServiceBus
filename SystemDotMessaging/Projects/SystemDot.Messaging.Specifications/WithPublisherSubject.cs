using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing;

namespace SystemDot.Messaging.Specifications
{
    public class WithPublisherSubject : WithMessageConfigurationSubject
    {
        protected static void Subscribe(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            Server.ReceiveMessage(new MessagePayload().BuildSubscriptionRequest(subscriberAddress, publisherAddress)); 
        }
        
        protected static void SubscribeDurable(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            MessagePayload request = new MessagePayload().BuildSubscriptionRequest(subscriberAddress, publisherAddress);
            request.GetSubscriptionRequestSchema().IsDurable = true;
            Server.ReceiveMessage(request);
        }
    }
}