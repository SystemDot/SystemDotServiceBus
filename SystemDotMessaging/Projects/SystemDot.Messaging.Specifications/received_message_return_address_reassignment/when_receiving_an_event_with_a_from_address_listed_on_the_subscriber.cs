using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.received_message_return_address_reassignment
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_event_with_a_from_address_listed_on_the_subscriber : WithHttpServerConfigurationSubject
    {
        const string PublisherChannel = "PublisherChannel";
        const string PublisherServerName = "PublisherServerName";
        const string LocalPublisherServerAddress = "LocalPublisherServerAddress";
        const string SubscriberServerName = "SubscriberServerName";
        const string SubscriberChannel = "SubscriberChannel";
        
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            ServerAddressConfiguration.AddAddress(PublisherServerName, LocalPublisherServerAddress);
            
            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer("SubscriberServerName")
                .OpenChannel("SubscriberChannel").ForSubscribingTo(PublisherChannel)
                .Initialise();

            messagePayload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(PublisherChannel)
                .SetFromServer(PublisherServerName)
                .SetFromServerAddress("PublisherServerAddress")
                .SetToChannel(SubscriberChannel)
                .SetToServer(SubscriberServerName)
                .SetChannelType(PersistenceUseType.SubscriberReceive)
                .Sequenced();

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => SendMessageToServer(messagePayload);

        It should_send_the_acknoweldgement_to_the_local_address_listed = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetToAddress().Server.Address.Path.ShouldBeEquivalentTo(LocalPublisherServerAddress);
    }
}