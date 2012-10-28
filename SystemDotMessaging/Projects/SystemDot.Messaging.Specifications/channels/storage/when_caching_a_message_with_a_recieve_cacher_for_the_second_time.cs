using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage
{
    public class when_caching_a_message_with_a_recieve_cacher_for_the_second_time : WithSubject<ReceiveChannelMessageCacher>
    {
        static MessagePayload message;
        static MessagePersistenceId originalPersistenceId;

        Establish context = () =>
        {
            With<PersistenceBehaviour>();

            message = new MessagePayload();
            message.SetPersistenceId(
                new EndpointAddress("OriginalChannel", "OriginalServer"), 
                PersistenceUseType.ReplySend);
            
            originalPersistenceId = message.GetPersistenceId();
            Subject.MessageProcessed += _ => { };
        };

        Because of = () =>
        {
            Subject.InputMessage(message);
            Subject.InputMessage(message);
        };

        It should_not_change_the_last_persistence_id_on_the_message = () =>
            message.GetLastPersistenceId().ShouldEqual(originalPersistenceId);
    }
}