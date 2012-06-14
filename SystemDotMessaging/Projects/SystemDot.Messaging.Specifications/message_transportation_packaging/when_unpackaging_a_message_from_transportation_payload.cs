using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Messaging.Recieving;
using SystemDot.Pipes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_transportation_packaging
{
    public class when_unpackaging_a_message_from_transportation_payload
    {
        static MessagePayloadUnpackager packager;
        static Pipe<MessagePayload> inputPipe;
        static Pipe<object> outputPipe;
        static object message;
        static object pushedMessage;
        static MessagePayload messagePayload;
        
        Establish context = () =>
        {
            inputPipe = new Pipe<MessagePayload>();
            outputPipe = new Pipe<object>();
            outputPipe.ItemPushed += i => pushedMessage = i;
            packager = new MessagePayloadUnpackager(inputPipe, outputPipe);

            message = new object();
            messagePayload = new MessagePayload("Test");
            messagePayload.AddHeader(new BodyMessageHeader(message)); 
        };

        Because of = () => inputPipe.Push(messagePayload);

        It should_unpack_the_message_from_the_payload_and_send_it_to_the_output_pipe = () =>
            pushedMessage.ShouldBeTheSameAs(message);
    }

    
}