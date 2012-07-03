using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Messages.Processing;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.processing
{
    [Subject("Message processing")]
    public class when_addressing_a_message : WithSubject<MessageAddresser>
    {
        static MessagePayload message;
        static MessagePayload processedPayload;
        static EndpointAddress address;

        Establish context = () =>
        {
            address = new EndpointAddress("Test");
            Configure<EndpointAddress>(address);
            Subject.MessageProcessed += i => processedPayload = i;
            message = new MessagePayload();
        };

        Because of = () => Subject.InputMessage(message);

        It should_set_the_default_address_of_the_message = () =>
            processedPayload.GetToAddress().ShouldEqual(address);
    }
}