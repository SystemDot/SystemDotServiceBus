using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Publishing;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications
{
    public class WithPublisherSubject : WithMessageConfigurationSubject
    {
        protected static void Subscribe(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            Server.ReceiveMessage(BuildSubscriptionRequest(subscriberAddress, publisherAddress));
        }
        
        protected static void SubscribeDurable(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            MessagePayload request = BuildSubscriptionRequest(subscriberAddress, publisherAddress);
            request.GetSubscriptionRequestSchema().IsDurable = true;
            Server.ReceiveMessage(request);
        }

        protected static MessagePayload BuildSubscriptionRequest(
            EndpointAddress subscriberAddress, 
            EndpointAddress publisherAddress)
        {
            var request = new MessagePayload();
            request.SetFromAddress(subscriberAddress);
            request.SetToAddress(publisherAddress);
            request.SetSubscriptionRequest(new SubscriptionSchema { SubscriberAddress = subscriberAddress });
            request.SetPersistenceId(subscriberAddress, PersistenceUseType.SubscriberRequestSend);
            request.SetSourcePersistenceId(request.GetPersistenceId());

            return request;
        }
    }
}