using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing.load_balancing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_twenty_one_messages_that_are_not_yet_acknowledged : WithPublisherSubject
    {
        const string PublisherAddress = "PublisherAddress";
        static List<int> messages;
        

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(PublisherAddress)
                .ForPublishing()
                .Initialise();

            Subscribe(BuildAddress("SubscriberAddress"), BuildAddress(PublisherAddress));
            messages = Enumerable.Range(1, 21).ToList();
        };

        Because of = () => messages.ForEach(m => Bus.Publish(m));

        It should_not_send_the_twenty_first_message = () => Server.SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(20);
    }
}
