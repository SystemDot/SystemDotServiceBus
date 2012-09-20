using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Messages.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.storage
{
    [Subject("Message caching")]
    public class when_caching_a_message : WithSubject<MessageCacher>
    {
        static MessagePayload processedMessage;
        static MessagePayload message;

        Establish context = () =>
        {
            Configure<EndpointAddress>(new EndpointAddress("Channel", "Server"));
            message = new MessagePayload();
            message.SetFromAddress(The<EndpointAddress>());

            Configure<IMessageCache>(new MessageCache(new InMemoryPersistence(), The<EndpointAddress>()));
            Subject.MessageProcessed += m => processedMessage = m; 
        };

        Because of = () => Subject.InputMessage(message);

        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(message);

        It should_cache_the_message = () => The<IMessageCache>().GetAll().ShouldContain(message);
    }
}