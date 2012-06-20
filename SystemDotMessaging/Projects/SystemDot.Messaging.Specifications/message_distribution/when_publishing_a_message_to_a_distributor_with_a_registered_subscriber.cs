using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.MessageTransportation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_distribution
{
    [Subject("Message distribution")]
    public class when_publishing_a_message_to_a_distributor_with_a_registered_subscriber : WithSubject<Distributor>
    {
        static MessagePayload inputMessage;
        static DistributionSubscriber distributionSubscriber;
        static MessagePayload publishedMessage;
        static MessagePayload message;

        Establish context = () =>
        {
            Configure<MessagePayloadCopier>(new MessagePayloadCopier());
            distributionSubscriber = new DistributionSubscriber();
            distributionSubscriber.MessageProcessed += m => publishedMessage = m;
            Subject.Subscribe(distributionSubscriber);

            message = new MessagePayload(new Address("TestAddress"));
            inputMessage = message;
        };

        Because of = () => Subject.InputMessage(inputMessage);

        It should_pass_an_equivelent_message_to_the_subscriber = () => publishedMessage.Address.ShouldEqual(message.Address);

        It should_copy_the_message_to_the_subscriber = () => publishedMessage.ShouldNotBeTheSameAs(message);

    }
}