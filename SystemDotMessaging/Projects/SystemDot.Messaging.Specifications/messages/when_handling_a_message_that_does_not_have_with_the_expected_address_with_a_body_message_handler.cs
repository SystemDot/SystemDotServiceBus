using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Messages.Processing;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages
{
    [Subject("Message handling")]
    public class when_handling_a_message_that_does_not_have_with_the_expected_address_with_a_body_message_handler : WithSubject<BodyMessageHandler>
    {
        static MessagePayload message;
        static MessagePayload processedMessage;

        Establish context = () =>
        {
            Subject = new BodyMessageHandler(new EndpointAddress("Channel", "Server"));
            Subject.MessageProcessed += m => processedMessage = m;
            
            message = new MessagePayload();
            message.SetToAddress(new EndpointAddress("OtherChannel", "OtherServer"));
            message.SetBody(new byte[0]);
        };

        Because of = () => Subject.InputMessage(message);

        It should_not_let_the_message_pass_through = () => processedMessage.ShouldBeNull();
    }
}