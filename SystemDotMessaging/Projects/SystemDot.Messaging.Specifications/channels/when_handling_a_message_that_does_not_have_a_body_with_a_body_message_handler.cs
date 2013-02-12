using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Packaging;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels
{
    [Subject("Message handling")]
    public class when_handling_a_message_that_does_not_have_a_body_with_a_body_message_handler : WithSubject<BodyMessageFilter>
    {
        static MessagePayload message;
        static MessagePayload processedMessage; 

        Establish context = () =>
        {
            Subject = new BodyMessageFilter(TestEndpointAddressBuilder.Build("GetChannel", "Server"));
            Subject.MessageProcessed += m => processedMessage = m; 
            message = new MessagePayload();
        };

        Because of = () => Subject.InputMessage(message);

        It should_not_let_the_message_pass_through = () => processedMessage.ShouldBeNull();
    }
}
