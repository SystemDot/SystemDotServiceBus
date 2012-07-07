using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Messaging.Transport.Http.LongPolling.Servers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.long_polling.serving
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

            sentMessageInQueue1 = new MessagePayload();
            sentMessageInQueue1.SetToAddress(new EndpointAddress("Address2"));
            outgoingQueue.Enqueue(sentMessageInQueue1);

            sentMessageInQueue2 = new MessagePayload();
            sentMessageInQueue2.SetToAddress(new EndpointAddress("Address2"));
            outgoingQueue.Enqueue(sentMessageInQueue2);

            longPollRequest = new MessagePayload();
            longPollRequest.SetLongPollRequest(new List<EndpointAddress> { new EndpointAddress("Address2") });


            inputStream.Serialise(longPollRequest, formatter);
        };

        Because of = () => server.HandleRequest(inputStream, outputStream);

        It should_put_the_first_message_in_the_response_stream = () =>
            outputStream.Deserialise<IEnumerable<MessagePayload>>(formatter)
                .First().GetToAddress().ShouldEqual(sentMessageInQueue1.GetToAddress());
        
        It should_put_the_second_message_in_the_response_stream = () => 
            outputStream.Deserialise<IEnumerable<MessagePayload>>(formatter)
                .Last().GetToAddress().ShouldEqual(sentMessageInQueue2.GetToAddress());
    }
}