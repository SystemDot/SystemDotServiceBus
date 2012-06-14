using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.Servers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_serving
{
    [Subject("Message serving")]
    public class when_responding_to_a_request_with_a_message_handler
    {
        static HttpMessagingServer server;
        static BinaryFormatter formatter; 
        static MemoryStream inputStream;
        static MemoryStream outputStream;
        static MessagePayload outputPayload1;
        static MessagePayload outputPayload2;
        static TestMessagingServerHandler handler; 
        
        Establish context = () =>
        {
            formatter = new BinaryFormatter();
            inputStream = new MemoryStream();
            formatter.Serialize(inputStream, new MessagePayload("TestAddress"));
            inputStream.Seek(0, 0);  

            outputStream = new MemoryStream();
            outputPayload1 = new MessagePayload("TestAddress1");
            outputPayload2 = new MessagePayload("TestAddress2");

            handler = new TestMessagingServerHandler(outputPayload1, outputPayload2);
            server = new HttpMessagingServer(formatter, handler);
        };

        Because of = () => server.HandleRequest(inputStream, outputStream);

        It should_put_the_first_message_in_the_response_stream = () =>
        {
            outputStream.Seek(0, 0);
            formatter.Deserialize(outputStream).As<IEnumerable<MessagePayload>>()
                .First().Address.ShouldEqual(outputPayload1.Address);
        };
        
        It should_put_the_second_message_in_the_response_stream = () =>
        {
            outputStream.Seek(0, 0);
            formatter.Deserialize(outputStream).As<IEnumerable<MessagePayload>>()
                .Last().Address.ShouldEqual(outputPayload2.Address);
        };
    }
}