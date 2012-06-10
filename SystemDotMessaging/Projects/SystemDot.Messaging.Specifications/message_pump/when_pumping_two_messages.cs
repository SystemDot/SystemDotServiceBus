using System.Collections.Generic;
using SystemDot.Messaging.Pipes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_pump
{
    public class when_pumping_two_messages
    {
        static MessagePump pump;
        static object message1;
        static object message2;
        static List<object> messages;

        Establish context = () =>
        {
            messages = new List<object>();
            message1 = new object();
            message2 = new object();
            
            pump = new MessagePump();
            pump.MessagePublished += m => messages.Add(m);

            pump.Publish(message1);
            pump.Publish(message2);
        };

        Because of = () => pump.PerformWork();

        It should_pump_the_messages = () => messages.ShouldContain(message1, message2);
    }
}