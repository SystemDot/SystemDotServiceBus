using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;
using SystemDot.Messaging.Publishing;

namespace SystemDot.Messaging.Specifications.transport.http.received_message_return_address_reassignment
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_subscription_request_with_a_from_proxy_address_listed_on_the_publisher : WithServerConfigurationSubject
    {
        const string PublisherChannel = "PublisherChannel";
        const string PublisherServerName = "PublisherServerName";
        const string LocalPublisherProxyAddress = "LocalPublisherProxyAddress";
        const string SubscriberChannel = "SubscriberChannel";
        const string SubscriberProxyName = "SubscriberProxyName";
        
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            ServerAddressConfiguration.AddAddress(SubscriberProxyName, LocalPublisherProxyAddress);
            
            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer(PublisherServerName)
                .OpenChannel(PublisherChannel).ForPublishing()
                .Initialise();

            messagePayload = new MessagePayload()
                .SetFromChannel(SubscriberChannel)
                .SetFromProxy(SubscriberProxyName)
                .SetFromProxyAddress("SubscriberProxyAddress")
                .SetToChannel(PublisherChannel)
                .SetToServer(PublisherServerName)
                .SetChannelType(PersistenceUseType.SubscriberRequestReceive)
                .SetSubscriptionRequest();
        };

        Because of = () => SendMessagesToServer(messagePayload);

        It should_send_the_acknoweldgement_to_the_local_proxy_address_listed = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Route.Proxy.Address.Path.ShouldEqual(LocalPublisherProxyAddress);
    }
}