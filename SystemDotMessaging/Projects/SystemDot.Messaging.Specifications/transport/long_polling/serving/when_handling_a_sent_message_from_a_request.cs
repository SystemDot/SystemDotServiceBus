using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.LongPolling.Servers;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.long_polling.serving
{
    [Subject("Message serving")]
    public class when_handling_a_sent_message_from_a_request
    {
        static HttpMessagingServer server;
        static ISerialiser formatter; 
        static MemoryStream stream;
        static MessagePayload sentMessage;
        static MessagePayloadQueue outgoingQueue;

        Establish context = () =>
        {
            formatter = new PlatformAgnosticSerialiser();
            outgoingQueue = new MessagePayloadQueue(new TimeSpan(0, 0, 0));
            stream = new MemoryStream();
            
            server = new HttpMessagingServer(
                formatter, 
                new SentMessageHandler(outgoingQueue),
                new LongPollHandler(outgoingQueue));
            
            sentMessage = new MessagePayload();
            sentMessage.SetToAddress(new EndpointAddress("Message"));
            sentMessage.SetBody(new byte[0]);

            stream.Serialise(sentMessage, formatter);           
        };

        Because of = () => server.HandleRequest(stream, new MemoryStream());

        It should_pass_the_message_to_the_outgoing_queue_ = () =>
            outgoingQueue.DequeueAll(sentMessage.GetToAddress()).First().GetToAddress().ShouldEqual(sentMessage.GetToAddress());
    }

}