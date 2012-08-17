using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.bus
{
    [Subject("Message bus")]
    public class when_replying_with_a_message_on_the_bus : WithSubject<MessageBus>
    {
        static object message;
        static object processedMessage;
        
        Establish context = () =>
        {
            Subject.MessageReplied += m => processedMessage = m; 
            message = new object();
        };

        Because of = () => Subject.Reply(message);

        It should_reply_with_the_message = () => processedMessage.ShouldBeTheSameAs(message);
    }
}
