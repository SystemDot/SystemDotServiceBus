using System.Collections.Generic;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.repeating
{
    public class when_starting_a_repeater_with_a_simple_repeat_strategy_with_messages_in_the_persistence : WithSubject<MessageRepeater>
    {
        static List<MessagePayload> messages;
        static MessagePayload message1;
        static MessagePayload message2;

        Establish context = () =>
        {
            With<PersistenceBehaviour>();
            Configure<IRepeatStrategy>(new SimpleRepeatStrategy(The<IPersistence>()));
            
            messages = new List<MessagePayload>();
                
            message1 = new MessagePayload();
            The<IPersistence>().AddMessage(message1);

            message2 = new MessagePayload();
            The<IPersistence>().AddMessage(message2);

            Subject.MessageProcessed += m => messages.Add(m);
        };

        Because of = () => Subject.Start();

        It should_repeat_the_messages = () => messages.ShouldContain(message1, message2);
    }
}