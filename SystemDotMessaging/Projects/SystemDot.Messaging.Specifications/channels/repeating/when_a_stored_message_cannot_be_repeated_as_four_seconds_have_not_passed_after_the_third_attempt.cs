using System;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using SystemDot.Specifications;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.repeating
{
    public class when_a_stored_message_cannot_be_repeated_as_four_seconds_have_not_passed_after_the_third_attempt
        : WithSubject<MessageRepeater>
    {
        static MessagePayload processedMessage;
        static MessagePayload message;

        Establish context = () =>
        {
            var endpointAddress = new EndpointAddress("Channel", "Server");
            Configure<IPersistence>(new InMemoryPersistence());
            Configure<IMessageCache>(new MessageCache(The<IPersistence>()));
            
            DateTime currentDate = DateTime.Now;
            Configure<ICurrentDateProvider>(new TestCurrentDateProvider(currentDate));

            Subject.MessageProcessed += m => processedMessage = m;

            message = new MessagePayload();
            message.SetFromAddress(endpointAddress);
            message.SetLastTimeSent(currentDate.Subtract(new TimeSpan(0, 0, 0, 0, 3999)));
            message.IncreaseAmountSent();
            The<IMessageCache>().Cache(message);
            message.IncreaseAmountSent();
            message.IncreaseAmountSent();
        };

        Because of = () => Subject.Start();

        It should_output_the_message = () => processedMessage.ShouldBeNull();
    }
}