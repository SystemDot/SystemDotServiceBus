using System;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.repeating
{
    [Subject("Repeating")]
    public class when_inputting_items_into_a_repeater_with_no_listener : WithSubject<MessageRepeater>
    {
        static Exception exception;
        
        Because of = () => exception = Catch.Exception(() => Subject.InputMessage(new MessagePayload()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}