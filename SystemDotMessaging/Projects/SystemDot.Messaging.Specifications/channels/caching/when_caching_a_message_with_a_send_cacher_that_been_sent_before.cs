using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching
{
    public class when_caching_a_message_with_a_send_cacher_that_been_sent_before : WithSubject<SendChannelMessageCacher>
    {
        static MessagePayload message;

        Establish context = () =>
            {
                With<PersistenceBehaviour>();

                message = new MessagePayload();
                message.IncreaseAmountSent();
                message.IncreaseAmountSent();
            };

        Because of = () => Subject.InputMessage(message);

        It should_not_increment_the_persistence_sequence = () => The<MessageCache>().GetSequence().ShouldEqual(1);
    }
}