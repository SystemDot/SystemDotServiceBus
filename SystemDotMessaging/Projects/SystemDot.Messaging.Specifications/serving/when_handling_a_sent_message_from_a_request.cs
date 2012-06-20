using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Messaging.Servers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.serving
{
    [Subject("Message serving")]
    public class when_handling_a_sent_message_from_a_request
    {
        static HttpMessagingServer server;
        static BinaryFormatter formatter; 
        static MemoryStream stream;
        static MessagePayload sentMessage;
        static MessagePayloadQueue outgoingQueue;

        Establish context = () =>
        {
            formatter = new BinaryFormatter();
            outgoingQueue = new MessagePayloadQueue(new TimeSpan(0, 0, 0));
            stream = new MemoryStream();
            
            server = new HttpMessagingServer(
                formatter, 
                new SentMessageHandler(outgoingQueue),
                new LongPollHandler(outgoingQueue));
            
            sentMessage = new MessagePayload(new Address("Message"));
            sentMessage.SetBody(new byte[0]);

            stream.Serialise(sentMessage, formatter);           
        };

        Because of = () => server.HandleRequest(stream, new MemoryStream());

        It should_pass_the_message_to_the_outgoing_queue_ = () => 
            outgoingQueue.DequeueAll(sentMessage.Address).First().Address.ShouldEqual(sentMessage.Address);
    }

}