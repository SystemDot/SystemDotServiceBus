using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.received_message_return_address_reassignment
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_subscription_request_with_a_from_address_listed_on_the_publisher : WithHttpServerConfigurationSubject
    {
        const string PublisherChannel = "PublisherChannel";
        const string PublisherServerName = "PublisherServerName";
        const string LocalPublisherServerAddress = "LocalPublisherServerAddress";
        const string SubscriberChannel = "SubscriberChannel";
        const string SubscriberServerName = "SubscriberServerName";
        
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            ServerAddressConfiguration.AddAddress(SubscriberServerName, LocalPublisherServerAddress);
            
            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer(PublisherServerName)
                .OpenChannel(PublisherChannel).ForPublishing()
                .Initialise();

            messagePayload = new MessagePayload()
                .SetFromChannel(SubscriberChannel)
                .SetFromServer(SubscriberServerName)
                .SetFromServerAddress("SubscriberServerAddress")
                .SetToChannel(PublisherChannel)
                .SetToServer(PublisherServerName)
                .SetSubscriptionRequest();
        };

        Because of = () => SendMessageToServer(messagePayload);

        It should_send_the_acknoweldgement_to_the_local_address_listed = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Server.Address.Path.ShouldEqual(LocalPublisherServerAddress);
    }
}