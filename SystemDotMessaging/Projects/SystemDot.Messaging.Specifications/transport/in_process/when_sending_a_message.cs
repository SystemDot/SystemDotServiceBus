using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.InProcess;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.in_process
{
    [Subject("In process transport")]
    public class when_sending_a_message : WithSubject<MessageSender>
    {
        static MessagePayload processed;
        static MessagePayload message;

        Establish context = () =>
        {
            message = new MessagePayload();
            message.SetToAddress(new EndpointAddress("GetChannel", "Server"));
            Subject = new MessageSender(The<InProcessMessageServer>());
            The<InProcessMessageServer>().MessageProcessed += m => processed = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_any_messages_passed_through_the_server = () => processed.ShouldBeTheSameAs(message);
    }
}