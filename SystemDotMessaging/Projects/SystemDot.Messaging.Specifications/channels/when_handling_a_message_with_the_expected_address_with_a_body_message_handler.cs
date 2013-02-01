using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels
{
    [Subject("Message handling")]
    public class when_handling_a_message_with_the_expected_address_with_a_body_message_handler : WithSubject<BodyMessageFilter>
    {
        static MessagePayload message;
        static MessagePayload processedMessage;
        static EndpointAddress address;

        Establish context = () =>
        {
            address = new EndpointAddress("GetChannel", "Server");
            
            Subject = new BodyMessageFilter(address);
            Subject.MessageProcessed += m => processedMessage = m;
            
            message = new MessagePayload();
            message.SetToAddress(address);
            message.SetBody(new byte[0]);
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processedMessage.ShouldBeTheSameAs(message);
    }
}