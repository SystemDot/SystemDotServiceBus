using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.packaging
{
    [Subject("Message processing")]
    public class when_packaging_a_message_into_transportation_payload
    {
        static MessagePayloadPackager packager;
        static ISerialiser serialiser;
        static MessagePayload processedPayload;
        static string message;
        
        Establish context = () =>
        {
            serialiser = new PlatformAgnosticSerialiser();

            packager = new MessagePayloadPackager(serialiser);
            packager.MessageProcessed += i => processedPayload = i;
            
            message = "test";
        };

        Because of = () => packager.InputMessage(message);

        It should_send_the_message_to_the_bus_output_pipe = () =>
            serialiser.Deserialise(processedPayload.GetHeader<BodyHeader>().Body).ShouldEqual(message);

        
    }
}