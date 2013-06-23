using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing.Builders;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.publishing_subscription
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_subscription_request_message_on_a_subscription_channel 
        : WithPublisherSubject
    {
        const string PublisherAddress = "PublisherAddress";
        const string SubscriberAddress = "SubscriberAddress";

        static MessagePayload request;
        static SubscriberSendChannelBuilt channelBuiltEvent;
        
        Establish context = () =>
        {
            Messenger.Register<SubscriberSendChannelBuilt>(e => channelBuiltEvent = e);
            
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(PublisherAddress).ForPublishing()
                .Initialise();

            request = new MessagePayload().BuildSubscriptionRequest(BuildAddress(SubscriberAddress), BuildAddress(PublisherAddress));
        };

        Because of = () => Server.ReceiveMessage(request);

        It should_notify_that_the_channel_was_built = () =>
           channelBuiltEvent.ShouldMatch(m =>
               m.CacheAddress == BuildAddress(SubscriberAddress)
               && m.SubscriberAddress == BuildAddress(SubscriberAddress)
               && m.PublisherAddress == BuildAddress(PublisherAddress));

        It should_send_an_acknowledgement_for_the_request = () => 
            Server.SentMessages.ShouldContain(a => a.GetAcknowledgementId() == request.GetPersistenceId());
    }
}