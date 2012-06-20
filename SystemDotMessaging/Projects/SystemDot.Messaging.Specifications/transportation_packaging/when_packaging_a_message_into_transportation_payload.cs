using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.Channels.Local;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transportation_packaging
{
    [Subject("Message transportation packaging")]
    public class when_packaging_a_message_into_transportation_payload
    {
        static MessagePayloadPackager packager;
        static ISerialiser serialiser;
        static MessagePayload processedPayload;
        static string message;
        
        Establish context = () =>
        {
            serialiser = new BinarySerialiser(new BinaryFormatter());

            packager = new MessagePayloadPackager(serialiser);
            packager.MessageProcessed += i => processedPayload = i;
            
            message = "test";
        };

        Because of = () => packager.InputMessage(message);

        It should_set_the_default_address_of_the_message = () =>
            processedPayload.Address.ShouldEqual(Address.Default);

        It should_send_the_message_to_the_bus_output_pipe = () =>
            serialiser.Deserialise(processedPayload.Headers.OfType<BodyHeader>().First().Body).ShouldEqual(message);

        
    }
}