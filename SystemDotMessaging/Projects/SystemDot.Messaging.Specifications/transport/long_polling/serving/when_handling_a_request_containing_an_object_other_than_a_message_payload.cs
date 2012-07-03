using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.Transport.Http.LongPolling.Servers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.long_polling.serving
{
    [Subject("Message serving")]
    public class when_handling_a_request_containing_an_object_other_than_a_message_payload
    {
        static HttpMessagingServer server;
        static BinaryFormatter formatter; 
        static MemoryStream inputStream;
        static MemoryStream outputStream;
        static MessagePayloadQueue outgoingQueue;
        static Exception exception;

        Establish context = () =>
        {
            formatter = new BinaryFormatter();
            outgoingQueue = new MessagePayloadQueue(new TimeSpan(0, 0, 0));
            inputStream = new MemoryStream();
            outputStream = new MemoryStream();

            inputStream.Serialise(new object(), formatter);

            server = new HttpMessagingServer(
                formatter,
                new SentMessageHandler(outgoingQueue),
                new LongPollHandler(outgoingQueue));
        };

        Because of = () => exception = Catch.Exception(() => server.HandleRequest(inputStream, outputStream));

        It should_not_fail = () => exception.ShouldBeNull();         
    }
}