using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Local;
using SystemDot.Messaging.MessageTransportation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.publishing
{
    public class when_retreving_a_registered_publisher : WithSubject<PublisherRegistry>
    {
        static IDistributor distributor;
        static Address address;
        static IDistributor retreived;
        
        Establish context = () =>
        {
            distributor = new Distributor(new MessagePayloadCopier());
            address = new Address("TestAddress");
            Subject.RegisterPublisher(address, distributor);
        };

        Because of = () => retreived = Subject.GetPublisher(address);

        It should_retreive_the_publisher_with_the_correct_address = () => retreived.ShouldBeTheSameAs(distributor);
    }
}