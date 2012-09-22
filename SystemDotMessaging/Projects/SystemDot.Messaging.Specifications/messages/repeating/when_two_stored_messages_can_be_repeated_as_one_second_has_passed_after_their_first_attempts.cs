using System;
using System.Collections.Generic;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Messages.Processing.Caching;
using SystemDot.Messaging.Messages.Processing.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Specifications;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.repeating.durable
{
    public class when_two_stored_messages_can_be_repeated_as_one_second_has_passed_after_their_first_attempts 
        : WithSubject<MessageRepeater>
    {
        static List<MessagePayload> processedMessages;
        static MessagePayload message1;
        static MessagePayload message2;

        Establish context = () =>
        {
            processedMessages = new List<MessagePayload>();
            DateTime currentDate = DateTime.Now;

            var endpointAddress = new EndpointAddress("Channel", "Server");
            Configure<IMessageCache>(new MessageCache(new InMemoryPersistence(), endpointAddress));
            Configure<ICurrentDateProvider>(new TestCurrentDateProvider(currentDate));

            Subject.MessageProcessed += m => processedMessages.Add(m);

            message1 = new MessagePayload();
            message1.SetFromAddress(endpointAddress);
            message1.SetLastTimeSent(currentDate.Subtract(new TimeSpan(0, 0, 0, 1)));
            message1.IncreaseAmountSent();
            The<IMessageCache>().Cache(message1);

            message2 = new MessagePayload();
            message2.SetFromAddress(endpointAddress);
            message2.SetLastTimeSent(currentDate.Subtract(new TimeSpan(0, 0, 0, 1)));
            message2.IncreaseAmountSent();
            The<IMessageCache>().Cache(message2);
        };

        Because of = () => Subject.Start();

        It should_both_messages = () => processedMessages.ShouldContain(message1, message2);
    }
}