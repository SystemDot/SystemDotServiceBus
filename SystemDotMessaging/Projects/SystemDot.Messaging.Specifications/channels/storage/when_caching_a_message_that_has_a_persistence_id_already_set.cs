using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage
{
    [Subject("Message caching")]
    public class when_caching_a_message_that_has_a_persistence_id_already_set : WithSubject<MessageCacher>
    {
        static MessagePayload message;

        Establish context = () =>
            {
                message = new MessagePayload();
                message.SetPersistenceId(new EndpointAddress("OtherChannel", "OtherServer"), PersistenceUseType.Other);
                message.IncreaseAmountSent();

                With<PersistenceBehaviour>();
                Configure<IMessageCache>(new MessageCache(The<IPersistence>()));

                Subject.MessageProcessed += _ => { };
            };

        Because of = () => Subject.InputMessage(message);

        It should_set_the_correct_persistence_id_on_the_message = () =>
            message.GetPersistenceId()
                .ShouldEqual(new MessagePersistenceId(
                    message.Id,
                    The<IMessageCache>().Address,
                    The<IMessageCache>().UseType));
    }
}