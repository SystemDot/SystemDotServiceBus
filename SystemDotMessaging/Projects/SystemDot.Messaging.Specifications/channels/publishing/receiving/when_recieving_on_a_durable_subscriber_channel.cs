using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_recieving_on_a_durable_subscriber_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        static MessagePayload payload;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForSubscribingTo(PublisherName)
                    .WithDurability()
                .Initialise();

            payload = new MessagePayload().MakeSequencedReceivable(
                1, 
                PublisherName, 
                ChannelName, 
                PersistenceUseType.SubscriberSend);
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_have_persisted_the_message = () =>
            Resolve<IChangeStore>()
                .GetAddedMessages(PersistenceUseType.SubscriberReceive, BuildAddress(ChannelName))
                .ShouldContain(payload);
    }
}