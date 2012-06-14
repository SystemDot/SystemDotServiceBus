using System.Collections.Generic;
using SystemDot.Messaging.Pipes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_pump
{
    public class when_pumping_a_message
    {
        static MessagePump pump;
        static object message;
        static object publishedMessage;

        Establish context = () =>
        {
            message = new object();
            
            pump = new MessagePump(new TestThreadPool());
            pump.MessagePublished += m => publishedMessage = m;
        };

        Because of = () => pump.Publish(message);

        It should_pump_the_message = () => publishedMessage.ShouldBeTheSameAs(message);
    }
}