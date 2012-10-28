using System;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.sequencing
{
    [Subject("Message processing")]
    public class when_passing_a_message_without_a_sequence_number_header_through_a_resequencer 
        : WithMessageProcessorSubject<Resequencer>
    {
        static Exception exception;

        Establish context = () =>
        {
            With<PersistenceBehaviour>();
            The<IPersistence>().SetSequence(1);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => Subject.InputMessage(new MessagePayload()));
        };

        It should_not_fail = () => exception.ShouldBeNull();
    }
}