using System.Linq;
using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.distribution
{
    [Subject("Message distribution")]
    public class when_publishing_a_message_to_a_distributor_with_two_registered_subscribers 
        : WithSubject<Distributor>
    {
        static MessagePayload inputMessage;
        static TestDistributionSubscriber subscriber1;
        static TestDistributionSubscriber subscriber2;
        static MessagePayload message;

        Establish context = () =>
        {
            subscriber1 = new TestDistributionSubscriber();
            Subject.Subscribe(subscriber1);

            subscriber2 = new TestDistributionSubscriber();
            Subject.Subscribe(subscriber2);
            
            message = new MessagePayload();
            message.SetToAddress(new Address("TestAddress"));
            inputMessage = message;
        };

        Because of = () => Subject.InputMessage(inputMessage);

        It should_pass_a_copy_of_the_message_to_the_first_subscriber = () =>
            subscriber1.ProcessedMessages.First().GetToAddress().ShouldEqual(message.GetToAddress());

        It should_pass_a_copy_of_the_message_to_the_second_subscriber = () =>
            subscriber2.ProcessedMessages.First().GetToAddress().ShouldEqual(message.GetToAddress());

    }
}