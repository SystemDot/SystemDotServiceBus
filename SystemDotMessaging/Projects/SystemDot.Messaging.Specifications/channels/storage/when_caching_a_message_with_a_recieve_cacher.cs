using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage
{
    public class when_caching_a_message_with_a_recieve_cacher : WithSubject<ReceiveChannelMessageCacher>
    {
        static MessagePayload message;
        static MessagePayload processedMessage;
        
        Establish context = () =>
        {
            With<PersistenceBehaviour>();
            message = new MessagePayload();
            message.IncreaseAmountSent();
            Subject.MessageProcessed += m => processedMessage = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_output_the_message = () => message.ShouldEqual(processedMessage);

        It should_not_increment_the_persistence_sequence = () =>
            The<IPersistence>().GetSequence().ShouldEqual(1);

        It should_set_the_correct_persistence_id_on_the_message = () =>
            message.GetPersistenceId()
                .ShouldEqual(new MessagePersistenceId(
                    message.Id,
                    The<IPersistence>().Address,
                    The<IPersistence>().UseType));

        It should_persist_the_message = () => The<IPersistence>().GetMessages().ShouldContain(m => message.Id == m.Id);
    }
}