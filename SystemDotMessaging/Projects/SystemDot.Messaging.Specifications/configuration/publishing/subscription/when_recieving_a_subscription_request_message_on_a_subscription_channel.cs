using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.configuration.publishing.subscription
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_subscription_request_message_on_a_subscription_channel 
        : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName = "TestSubscriber";

        static MessagePayload request;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            request = BuildSubscriptionRequest(BuildAddress(SubscriberName), BuildAddress(ChannelName));
        };

        Because of = () => MessageReciever.ReceiveMessage(request);

        It should_send_an_acknowledgement_for_the_request = () => 
            MessageSender.SentMessages.ShouldContain(a => a.GetAcknowledgementId() == request.GetPersistenceId());
    }
}