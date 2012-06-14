using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.Servers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_serving
{
    [Subject("Message serving")]
    public class when_handling_a_request_containing_a_message_payload_with_a_message_handler_that_should_not_handle_it
    {
        static HttpMessagingServer server;
        static BinaryFormatter formatter; 
        static MemoryStream stream;
        static MessagePayload payload;
        static TestMessagingServerHandler handler; 
        
        Establish context = () =>
        {
            formatter = new BinaryFormatter();
            handler = new TestMessagingServerHandler
            {
                HandlesMessages = false
            };

            server = new HttpMessagingServer(formatter, handler);

            stream = new MemoryStream();
            payload = new MessagePayload("TestAddress");

            formatter.Serialize(stream, payload);
            stream.Seek(0, 0);            
        };

        Because of = () => server.HandleRequest(stream, new MemoryStream());
        
        It should_not_pass_the_message_payload_to_the_handler = () => handler.HandledPayload.ShouldBeNull();
    }
}