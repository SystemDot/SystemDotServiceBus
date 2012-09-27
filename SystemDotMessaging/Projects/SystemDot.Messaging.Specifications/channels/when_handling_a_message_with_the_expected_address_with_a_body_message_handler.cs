using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
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
            address = new EndpointAddress("Channel", "Server");
            
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