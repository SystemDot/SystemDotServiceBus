using System.Linq;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.MessageTransportation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.distribution
{
    [Subject("Message distribution")]
    public class when_publishing_a_message_to_a_distributor_with_a_registered_subscriber : WithSubject<Distributor>
    {
        static MessagePayload inputMessage;
        static TestDistributionSubscriber distributionSubscriber;
        static MessagePayload message;

        Establish context = () =>
        {
            Configure<MessagePayloadCopier>(new MessagePayloadCopier());
            distributionSubscriber = new TestDistributionSubscriber();
            Subject.Subscribe(distributionSubscriber);

            message = new MessagePayload(new Address("TestAddress"));
            inputMessage = message;
        };

        Because of = () => Subject.InputMessage(inputMessage);

        It should_pass_an_equivelent_message_to_the_subscriber = () => 
            distributionSubscriber.ProcessedMessages.First().Address.ShouldEqual(message.Address);

        It should_copy_the_message_to_the_subscriber = () =>
            distributionSubscriber.ProcessedMessages.First().ShouldNotBeTheSameAs(message);

    }
}