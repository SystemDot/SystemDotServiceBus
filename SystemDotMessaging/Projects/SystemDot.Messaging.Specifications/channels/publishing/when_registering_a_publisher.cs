using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_retreving_a_registered_publisher : WithSubject<PublisherRegistry>
    {
        static IDistributor distributor;
        static EndpointAddress address;
        static IDistributor retreived;
        
        Establish context = () =>
        {
            distributor = new Distributor(new MessagePayloadCopier(new PlatformAgnosticSerialiser()));
            address = new EndpointAddress("TestAddress", "TestServer");
            Subject.RegisterPublisher(address, distributor);
        };

        Because of = () => retreived = Subject.GetPublisher(address);

        It should_retreive_the_publisher_with_the_correct_address = () => retreived.ShouldBeTheSameAs(distributor);
    }
}