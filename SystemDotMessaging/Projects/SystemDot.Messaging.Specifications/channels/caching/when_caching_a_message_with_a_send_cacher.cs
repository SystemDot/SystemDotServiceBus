using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching
{
    public class when_caching_a_message_with_a_send_cacher : WithSubject<SendChannelMessageCacher>
    {
        static MessagePayload message;
        static MessagePayload processedMessage;

        Establish context = () =>
        {
            With<PersistenceBehaviour>();

            message = new MessagePayload();
            message.SetSequence(1);
            message.IncreaseAmountSent();
            Subject.MessageProcessed += m => processedMessage = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_output_the_message = () => message.ShouldEqual(processedMessage);

        It should_set_the_correct_persistence_id_on_the_message = () =>
            message.GetPersistenceId()
                .ShouldEqual(new MessagePersistenceId(
                    message.Id,
                    The<MessageCache>().Address,
                    The<MessageCache>().UseType));

        It should_increment_the_persistence_sequence = () =>
            The<MessageCache>().GetSequence().ShouldEqual(2);

        It should_persist_the_message = () => The<MessageCache>().GetMessages().ShouldContain(m => message.Id == m.Id);
    }
}