using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.Messaging.Specifications.channels.storage
{
    [Subject("Message caching")]
    public class when_caching_a_message_for_the_first_time : WithSubject<MessageCacher>
    {
        static MessagePayload processedMessage;
        static MessagePayload message;

        Establish context = () =>
        {
            message = new MessagePayload();
            message.IncreaseAmountSent();

            With<PersistenceBehaviour>();
            Configure<IMessageCache>(new MessageCache(The<IPersistence>()));

            Subject.MessageProcessed += m => processedMessage = m; 
        };

        Because of = () => Subject.InputMessage(message);

        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(message);

        It should_cache_the_message = () => The<IMessageCache>().GetAll().ShouldContain(message);

        It should_set_the_correct_persistence_id_on_the_message = () => 
            message.GetPersistenceId()
                .ShouldEqual(new MessagePersistenceId(
                    message.Id,
                    The<IMessageCache>().Address, 
                    The<IMessageCache>().UseType));
    }
}