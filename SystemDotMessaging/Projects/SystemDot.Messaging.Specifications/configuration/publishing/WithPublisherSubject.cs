using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    public class WithPublisherSubject : WithNoRepeaterMessageConfigurationSubject
    {
        protected static void Subscribe(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            MessageReciever.RecieveMessage(BuildSubscriptionRequest(subscriberAddress, publisherAddress));
        }
        
        protected static void SubscribeDurable(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            MessagePayload request = BuildSubscriptionRequest(subscriberAddress, publisherAddress);
            request.GetSubscriptionRequestSchema().IsDurable = true;
            MessageReciever.RecieveMessage(request);
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