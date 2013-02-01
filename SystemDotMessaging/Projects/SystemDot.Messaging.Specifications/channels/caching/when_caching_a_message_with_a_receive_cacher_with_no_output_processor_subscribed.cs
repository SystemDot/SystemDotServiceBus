using System;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching
{
    [Subject("Message caching")]
    public class when_caching_a_message_with_a_receive_cacher_with_no_output_processor_subscribed
        : WithSubject<ReceiveChannelMessageCacher>
    {
        static MessagePayload message;
        static Exception exception;

        Establish context = () =>
        {
            With<PersistenceBehaviour>();
    
            message = new MessagePayload();
            message.IncreaseAmountSent();
        };

        Because of = () => exception = Catch.Exception(() => Subject.InputMessage(message));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}