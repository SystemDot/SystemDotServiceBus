using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Messaging.Recieving;
using SystemDot.Messaging.Sending;
using SystemDot.Messaging.Specifications.message_handling;
using SystemDot.Pipes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.message_transportation_packaging
{
    public class when_packaging_a_message_into_transportation_payload
    {
        static MessagePayloadPackager packager;
        static Pipe<object> inputPipe;
        static Pipe<MessagePayload> outputPipe;
        static MessagePayload pushedPayload;
        static object message;
        
        Establish context = () =>
        {
            inputPipe = new Pipe<object>();
            outputPipe = new Pipe<MessagePayload>();
            outputPipe.ItemPushed += i => pushedPayload = i;

            packager = new MessagePayloadPackager(inputPipe, outputPipe);
            
            message = new object();
        };

        Because of = () => inputPipe.Push(message);

        It should_set_the_default_address_of_the_message = () =>
            pushedPayload.Address.ShouldEqual("http://localhost/Default/");

        It should_send_the_message_to_the_bus_output_pipe = () =>
            pushedPayload.Headers.OfType<BodyMessageHeader>().First().Body.ShouldBeTheSameAs(message);
    }
}