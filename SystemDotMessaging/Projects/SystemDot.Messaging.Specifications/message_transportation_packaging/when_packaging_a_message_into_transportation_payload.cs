using System.Linq;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Messaging.Sending;
using SystemDot.Pipes;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_transportation_packaging
{
    [Subject("Message transportation packaging")]
    public class when_packaging_a_message_into_transportation_payload
    {
        static MessagePayloadPackager packager;
        static Pipe<object> inputPipe;
        static Pipe<MessagePayload> outputPipe;
        static ISerialiser serialiser;
        static MessagePayload pushedPayload;
        static string message;
        
        Establish context = () =>
        {
            inputPipe = new Pipe<object>();
            outputPipe = new Pipe<MessagePayload>();
            outputPipe.ItemPushed += i => pushedPayload = i;
            serialiser = new JsonSerialiser();

            packager = new MessagePayloadPackager(inputPipe, outputPipe, serialiser);
            
            message = "test";
        };

        Because of = () => inputPipe.Push(message);

        It should_set_the_default_address_of_the_message = () =>
            pushedPayload.Address.ShouldEqual("http://localhost:8090/Default/");

        It should_send_the_message_to_the_bus_output_pipe = () =>
            serialiser.Deserialise(pushedPayload.Headers.OfType<BodyHeader>().First().Body).ShouldEqual(message);

        
    }
}