using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.sequencing
{
    [Subject("Message processing")]
    public class when_passing_a_message_with_a_lower_sequence_of_that_of_the_resequencer : WithMessageProcessorSubject<Resequencer>
    {
        static MessagePayload message;
        
        Establish context = () =>
        {
            With<PersistenceBehaviour>();
            message = new MessagePayload();
            message.SetSequence(1);
            The<MessageCache>().SetSequence(2);
            The<MessageCache>().AddMessage(message);
        };

        Because of = () => Subject.InputMessage(message);

        It should_delete_the_message_from_persistence = () => The<MessageCache>().GetMessages().ShouldBeEmpty();
    }
}