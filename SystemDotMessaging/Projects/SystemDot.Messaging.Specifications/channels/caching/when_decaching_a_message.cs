using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching
{
    [Subject("Message caching")]
    public class when_decaching_a_message : WithSubject<MessageDecacher>
    {
        static MessagePayload message;
        static MessagePayload processedMessage;

        Establish context = () =>
        {
            message = new MessagePayload();
            message.IncreaseAmountSent();

            With<PersistenceBehaviour>();
            Subject.MessageProcessed += payload => processedMessage = payload;
        };

        Because of = () => Subject.InputMessage(message);

        It should_decache_the_message = () => The<MessageCache>().GetMessages().ShouldNotContain(message);

        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(message);
    }
}