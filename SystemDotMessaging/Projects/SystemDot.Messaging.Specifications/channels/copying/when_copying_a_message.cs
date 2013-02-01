using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.copying
{
    [Subject("Message distribution")]
    public class when_copying_a_message_payload : WithSubject<MessagePayloadCopier>
    {
        static MessagePayload payload;
        static MessagePayload copied;

        Establish context = () =>
        {
            Configure<ISerialiser>(new PlatformAgnosticSerialiser());
            payload = new MessagePayload();
            Subject.MessageProcessed += p => copied = p;
        };

        Because of = () => Subject.InputMessage(payload);

        It should_not_output_the_same_instance_of_the_message = () => copied.ShouldNotBeTheSameAs(payload);

        It should_copy_the_mesage = () => copied.ShouldEqual(payload);
    }
}