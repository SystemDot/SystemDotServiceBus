using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.server_addressing
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_subscription_request_message_on_a_subscription_channel : WithServerConfigurationSubject
    {
        const string PublisherServer = "PublisherServer";
        const string PublisherAddress = "PublisherAddress";
        const string SubscriberAddress = "SubscriberAddress";
        const string SubscriberServer = "SubscriberServer";
        const string SubscriberServerAddress = "SubscriberServerAddress";
        
        static MessagePayload request;
        
        Establish context = () =>
        {
            WebRequestor.ExpectAddress(SubscriberServer, SubscriberServerAddress);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(PublisherServer)
                .OpenChannel(PublisherAddress).ForPublishing()
                .Initialise();

            request = new MessagePayload().BuildSubscriptionRequest(
                BuildAddress(SubscriberAddress, SubscriberServer, SubscriberServer), 
                BuildAddress(PublisherAddress, PublisherServer, PublisherServer));
            
            request.SetFromServerAddress(SubscriberServerAddress);
        };

        Because of = () => SendMessagesToServer(request);

        It should_send_an_acknowledgement_to_the_server_address_specified_on_the_incoming_subscription = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().IsAcknowledgement();
    }
}