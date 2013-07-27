using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.received_message_return_address_reassignment
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_event_with_a_from_proxy_address_listed_on_the_subscriber : WithServerConfigurationSubject
    {
        const string PublisherChannel = "PublisherChannel";
        const string PublisherProxyName = "PublisherProxyName";
        const string LocalPublisherProxyAddress = "LocalPublisherProxyAddress";
        const string SubscriberServerName = "SubscriberServerName";
        const string SubscriberChannel = "SubscriberChannel";
        
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            ServerAddressConfiguration.AddAddress(PublisherProxyName, LocalPublisherProxyAddress);
            
            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer("SubscriberServerName")
                .OpenChannel("SubscriberChannel").ForSubscribingTo(PublisherChannel)
                .Initialise();

            messagePayload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(PublisherChannel)
                .SetFromProxy(PublisherProxyName)
                .SetFromProxyAddress("PublisherProxyAddress")
                .SetToChannel(SubscriberChannel)
                .SetToServer(SubscriberServerName)
                .SetChannelType(PersistenceUseType.SubscriberReceive)
                .Sequenced();

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => SendMessagesToServer(messagePayload);

        It should_send_the_acknoweldgement_to_the_local_proxy_address_listed = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Route.Proxy.Address.Path.ShouldEqual(LocalPublisherProxyAddress);
    }
}