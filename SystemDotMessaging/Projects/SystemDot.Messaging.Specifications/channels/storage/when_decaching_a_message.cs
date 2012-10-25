using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.Messaging.Specifications.channels.storage
{
    [Subject("Message caching")]
    public class when_decaching_a_message : WithSubject<MessageDecacher>
    {
        static MessagePayload message;

        Establish context = () =>
        {
            message = new MessagePayload();
            message.IncreaseAmountSent();
 
            With<PersistenceBehaviour>();
            Configure<IMessageCache>(new MessageCache(The<IPersistence>()));

            The<IMessageCache>().Cache(message);
        };

        Because of = () => Subject.InputMessage(message);

        It should_decache_the_message = () => The<IMessageCache>().GetAll().ShouldNotContain(message);
    }
}