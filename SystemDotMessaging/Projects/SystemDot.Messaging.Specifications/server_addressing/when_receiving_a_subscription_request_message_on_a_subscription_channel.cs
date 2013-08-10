using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.server_addressing
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_subscription_request_message_on_a_subscription_channel : WithHttpServerConfigurationSubject
    {
        const string PublisherServer = "PublisherServer";
        const string PublisherChannel = "PublisherChannel";
        const string SubscriberChannel = "SubscriberChannel";
        const string SubscriberServer = "SubscriberServer";
        const string SubscriberServerAddress = "SubscriberServerAddress";
        
        static MessagePayload request;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(PublisherServer)
                .OpenChannel(PublisherChannel).ForPublishing()
                .Initialise();

            request = new MessagePayload().BuildSubscriptionRequest(
                BuildAddress(SubscriberChannel, SubscriberServer, SubscriberServerAddress),
                BuildAddress(PublisherChannel, PublisherServer));
        };

        Because of = () => SendMessagesToServer(request);

        It should_send_an_acknowledgement_to_the_server_address_specified_on_the_incoming_subscription = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().IsAcknowledgement();
    }
}