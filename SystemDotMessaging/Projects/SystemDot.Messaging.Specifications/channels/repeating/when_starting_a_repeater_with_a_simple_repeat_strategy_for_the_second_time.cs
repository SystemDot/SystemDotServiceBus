using System.Collections.Generic;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.repeating
{
    public class when_starting_a_repeater_with_a_simple_repeat_strategy_for_the_second_time : WithSubject<MessageRepeater>
    {
        static List<MessagePayload> messages;
        static MessagePayload message1;
        
        Establish context = () =>
            {
                With<PersistenceBehaviour>();
                Configure<IRepeatStrategy>(new SimpleRepeatStrategy(The<MessageCache>()));
        
                messages = new List<MessagePayload>();

                message1 = new MessagePayload();
                The<MessageCache>().AddMessage(message1);
            
                Subject.MessageProcessed += m => messages.Add(m);
            };

        Because of = () =>
            {
                Subject.Start();
                Subject.Start();
            };

        It should_only_repeat_the_messages_once = () => messages.Count.ShouldEqual(1);
    }
}