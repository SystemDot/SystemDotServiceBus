using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Messaging.Recieving;
using SystemDot.Pipes;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_transportation_packaging
{
    public class when_unpackaging_a_message_from_transportation_payload
    {
        static MessagePayloadUnpackager packager;
        static Pipe<MessagePayload> inputPipe;
        static Pipe<object> outputPipe;
        static ISerialiser serialiser;
        static string message;
        static string pushedMessage;
        static MessagePayload messagePayload;
        
        Establish context = () =>
        {
            inputPipe = new Pipe<MessagePayload>();
            outputPipe = new Pipe<object>();
            outputPipe.ItemPushed += i => pushedMessage = (string)i;
            serialiser = new JsonSerialiser();
            packager = new MessagePayloadUnpackager(inputPipe, outputPipe, serialiser);
            
            message = "Test";
            messagePayload = new MessagePayload("TestAddress");
            messagePayload.AddHeader(new BodyMessageHeader(serialiser.Serialise(message))); 
        };

        Because of = () => inputPipe.Push(messagePayload);

        It should_unpack_the_message_from_the_payload_and_send_it_to_the_output_pipe = () => pushedMessage.ShouldEqual(message);
    }

    
}