using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels
{
    [Subject("Message processing")]
    public class when_unpackaging_a_message_from_transportation_payload
    {
        static MessagePayloadUnpackager packager;
        static ISerialiser serialiser;
        static string message;
        static string processedMessage;
        static MessagePayload messagePayload;
        
        Establish context = () =>
        {
            serialiser = new PlatformAgnosticSerialiser();
            packager = new MessagePayloadUnpackager(serialiser);
            packager.MessageProcessed += i => processedMessage = (string)i;
            
            message = "Test";
            messagePayload = new MessagePayload();
            messagePayload.SetBody(serialiser.Serialise(message)); 
        };

        Because of = () => packager.InputMessage(messagePayload);

        It should_unpack_the_message_from_the_payload_and_output_it = () => processedMessage.ShouldEqual(message);
    }

    
}