using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
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
            Configure<EndpointAddress>(new EndpointAddress("Channel", "Server"));
            message = new MessagePayload();
            message.SetFromAddress(The<EndpointAddress>());
            message.IncreaseAmountSent();

            Configure<IMessageCache>(new MessageCache(new TestPersistence(), The<EndpointAddress>()));
            The<IMessageCache>().Cache(message);
        };

        Because of = () => Subject.InputMessage(message);

        It should_cache_the_message = () => The<IMessageCache>().GetAll().ShouldNotContain(message);
    }
}