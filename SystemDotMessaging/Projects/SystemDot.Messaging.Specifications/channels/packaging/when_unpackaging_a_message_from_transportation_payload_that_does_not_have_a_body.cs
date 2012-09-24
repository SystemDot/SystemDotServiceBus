using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.packaging
{
    [Subject("Message processing")]
    public class when_unpackaging_a_message_from_transportation_payload_that_does_not_have_a_body
    {
        static MessagePayloadUnpackager packager;
        static ISerialiser serialiser;
        static object processedMessage;
        static MessagePayload messagePayload;
        
        Establish context = () =>
        {
            serialiser = new PlatformAgnosticSerialiser();
            packager = new MessagePayloadUnpackager(serialiser);
            packager.MessageProcessed += m => processedMessage = m;
            
            messagePayload = new MessagePayload();
        };

        Because of = () => packager.InputMessage(messagePayload);

        It should_not_unpack_the_message_from_the_payload_and_output_it = () => processedMessage.ShouldBeNull();
    }

    
}