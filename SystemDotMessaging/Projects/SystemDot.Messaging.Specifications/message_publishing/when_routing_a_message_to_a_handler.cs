using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.Sending;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_publishing
{
    [Subject("Message publishing")]
    public class when_publishing_a_message_to_a_hub_with_two_registed_subscribers
    {
        static PublisherHub hub;
        static MessagePayload inputMessage;

        Establish context = () =>
        {
            hub = new PublisherHub(); 
            inputMessage = new MessagePayload(new Address());
        };

        Because of = () => hub.InputMessage(inputMessage);

        It should_copy_the_message_and_pass_it_to_the_subscribers;
    }
}