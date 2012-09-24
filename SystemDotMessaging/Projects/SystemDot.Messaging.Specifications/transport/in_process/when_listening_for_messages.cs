using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Transport.InProcess;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.in_process
{
    [Subject("In process transport")]
    public class when_listening_for_messages : WithSubject<MessageReciever>
    {
        static MessagePayload processed;
        static MessagePayload message;
            
        Establish context = () =>
        {
            message = new MessagePayload();
            Subject = new MessageReciever(The<InProcessMessageServer>());
            Subject.MessageProcessed += m => processed = m;           
        };

        Because of = () => The<InProcessMessageServer>().InputMessage(message);
        
        It should_recieve_any_messages_passed_through_the_server = () => processed.ShouldBeTheSameAs(message);
    }
}