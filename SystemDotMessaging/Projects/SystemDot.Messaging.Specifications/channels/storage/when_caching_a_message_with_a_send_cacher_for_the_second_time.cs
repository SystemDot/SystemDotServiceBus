using System.Linq;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage
{
    public class when_caching_a_message_with_a_send_cacher_for_the_second_time : WithSubject<SendChannelMessageCacher>
    {
        static MessagePayload message;

        Establish context = () =>
            {
                With<PersistenceBehaviour>();

                message = new MessagePayload();
                message.IncreaseAmountSent();
            };

        Because of = () =>
            {
                Subject.InputMessage(message);
                Subject.InputMessage(message);
            };

        It should_add_the_source_persistence_header_a_second_time = () =>
            message.Headers.OfType<SourcePersistenceHeader>().Count().ShouldEqual(1);
    }
}