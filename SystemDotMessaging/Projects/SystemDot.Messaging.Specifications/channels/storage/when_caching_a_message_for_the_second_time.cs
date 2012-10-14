using System;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage
{
    [Subject("Message caching")]
    public class when_caching_a_message_for_the_second_time : WithSubject<MessageCacher>
    {
        static MessagePayload message;
        static Exception exception;

        Establish context = () =>
        {
            Configure(new EndpointAddress("Channel", "Server"));
            message = new MessagePayload();
            message.SetFromAddress(The<EndpointAddress>());
            message.IncreaseAmountSent();
            
            Configure<IPersistence>(new InMemoryPersistence());
            Configure<IMessageCache>(new MessageCache(The<IPersistence>()));

            Subject.MessageProcessed += m => { };

            Subject.InputMessage(message);
            message.IncreaseAmountSent();
        };

        Because of = () => exception = Catch.Exception(() => Subject.InputMessage(message));

        It should_not_fail = () => exception.ShouldBeNull();        
    }
}