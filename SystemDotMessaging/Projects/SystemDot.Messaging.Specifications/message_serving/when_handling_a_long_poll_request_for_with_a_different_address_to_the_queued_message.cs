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
    public class when_handling_a_long_poll_request_with_a_different_address_to_the_queued_message
    {
        static HttpMessagingServer server;
        static BinaryFormatter formatter; 
        static MemoryStream inputStream;
        static MemoryStream outputStream;
        static MessagePayload sentMessageInQueue;
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

            sentMessageInQueue = new MessagePayload("Address1");
            outgoingQueue.Enqueue(sentMessageInQueue);

            longPollRequest = new MessagePayload("Address2");
            longPollRequest.SetLongPollRequest();

            inputStream.Serialise(longPollRequest, formatter);
        };

        Because of = () => server.HandleRequest(inputStream, outputStream);

        It should_not_output_the_message_in_the_response_stream = () =>
            outputStream.Deserialise<IEnumerable<MessagePayload>>(formatter).ShouldBeEmpty();        
        
    }
}