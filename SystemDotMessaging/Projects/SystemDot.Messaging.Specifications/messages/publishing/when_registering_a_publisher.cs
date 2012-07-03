using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.publishing
{
    [Subject("Message publishing")]
    public class when_retreving_a_registered_publisher : WithSubject<PublisherRegistry>
    {
        static IDistributor distributor;
        static EndpointAddress address;
        static IDistributor retreived;
        
        Establish context = () =>
        {
            distributor = new Distributor(new MessagePayloadCopier());
            address = new EndpointAddress("TestAddress");
            Subject.RegisterPublisher(address, distributor);
        };

        Because of = () => retreived = Subject.GetPublisher(address);

        It should_retreive_the_publisher_with_the_correct_address = () => retreived.ShouldBeTheSameAs(distributor);
    }
}