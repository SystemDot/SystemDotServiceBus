using System;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
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
            Configure<EndpointAddress>(new EndpointAddress("Channel", "Server"));
            message = new MessagePayload();
            message.SetFromAddress(The<EndpointAddress>());
            message.IncreaseAmountSent();
            
            Configure<IMessageCache>(new MessageCache(new TestPersistence(), The<EndpointAddress>()));
            Subject.MessageProcessed += m => processedMessage = m; 
        };

        Because of = () => Subject.InputMessage(message);

        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(message);

        It should_cache_the_message = () => The<IMessageCache>().GetAll().ShouldContain(message);
    }

    [Subject("Message caching")]
    public class when_caching_a_message_for_the_second_time : WithSubject<MessageCacher>
    {
        static MessagePayload message;
        static Exception exception;

        Establish context = () =>
        {
            Configure<EndpointAddress>(new EndpointAddress("Channel", "Server"));
            message = new MessagePayload();
            message.SetFromAddress(The<EndpointAddress>());
            message.IncreaseAmountSent();
            
            Configure<IMessageCache>(new MessageCache(new TestPersistence(), The<EndpointAddress>()));
            Subject.MessageProcessed += m => { };

            Subject.InputMessage(message);
            message.IncreaseAmountSent();
        };

        Because of = () => exception = Catch.Exception(() => Subject.InputMessage(message));

        It should_not_fail = () => exception.ShouldBeNull();        
    }
}