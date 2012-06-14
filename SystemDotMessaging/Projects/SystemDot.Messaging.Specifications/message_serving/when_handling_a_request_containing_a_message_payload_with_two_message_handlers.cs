using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.Servers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_serving
{
    [Subject("Message serving")]
    public class when_handling_a_request_containing_a_message_payload_with_two_message_handlers
    {
        static HttpMessagingServer server;
        static BinaryFormatter formatter; 
        static MemoryStream stream;
        static MessagePayload payload;
        static TestMessagingServerHandler handler1; 
        static TestMessagingServerHandler handler2; 

        Establish context = () =>
        {
            formatter = new BinaryFormatter();
            handler1 = new TestMessagingServerHandler();
            handler2 = new TestMessagingServerHandler();
            server = new HttpMessagingServer(formatter, handler1, handler2);

            stream = new MemoryStream();
            payload = new MessagePayload("TestAddress");

            formatter.Serialize(stream, payload);
            stream.Seek(0, 0);            
        };

        Because of = () => server.HandleRequest(stream, new MemoryStream());

        It should_pass_the_message_payload_to_the_first_handler = () => handler1.HandledPayload.Address.ShouldEqual(payload.Address);

        It should_pass_the_message_payload_to_the_second_handler = () => handler2.HandledPayload.Address.ShouldEqual(payload.Address);
    }
}