using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Messaging.Servers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_serving
{
    [Subject("Message serving")]
    public class when_handling_a_long_poll_request_for_messages_with_two_messages_in_the_queue
    {
        static HttpMessagingServer server;
        static BinaryFormatter formatter; 
        static MemoryStream inputStream;
        static MemoryStream outputStream;
        static MessagePayload sentMessageInQueue1;
        static MessagePayload sentMessageInQueue2;
        static MessagePayload longPollRequest;
        static MessagePayloadQueue outgoingQueue;

        Establish context = () =>
        {
            formatter = new BinaryFormatter();
            outgoingQueue = new MessagePayloadQueue(new TimeSpan(0, 0, 0));
            inputStream = new MemoryStream();
            outputStream = new MemoryStream();
            
            server = new HttpMessagingServer(
                formatter,
                new SentMessageHandler(outgoingQueue),
                new LongPollHandler(outgoingQueue));

            sentMessageInQueue1 = new MessagePayload("Address1");
            outgoingQueue.Enqueue(sentMessageInQueue1);

            sentMessageInQueue2 = new MessagePayload("Address1");
            outgoingQueue.Enqueue(sentMessageInQueue2);

            longPollRequest = new MessagePayload("Address1");
            longPollRequest.SetLongPollRequest();

            inputStream.Serialise(longPollRequest, formatter);
        };

        Because of = () => server.HandleRequest(inputStream, outputStream);

        It should_put_the_first_message_in_the_response_stream = () =>
            outputStream.Deserialise<IEnumerable<MessagePayload>>(formatter)
                .First().Address.ShouldEqual(sentMessageInQueue1.Address);
        
        It should_put_the_second_message_in_the_response_stream = () => 
            outputStream.Deserialise<IEnumerable<MessagePayload>>(formatter)
                .Last().Address.ShouldEqual(sentMessageInQueue2.Address);
    }
}