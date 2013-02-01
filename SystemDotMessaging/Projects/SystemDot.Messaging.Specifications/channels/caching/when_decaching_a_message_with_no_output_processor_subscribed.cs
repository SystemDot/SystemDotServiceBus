using System;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching
{
    [Subject("Message caching")]
    public class when_decaching_a_message_with_no_output_processor_subscribed : WithSubject<MessageDecacher>
    {
        static Exception exception;

        Establish context = () => With<PersistenceBehaviour>();

        Because of = () => exception = Catch.Exception(() => Subject.InputMessage(new MessagePayload()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}