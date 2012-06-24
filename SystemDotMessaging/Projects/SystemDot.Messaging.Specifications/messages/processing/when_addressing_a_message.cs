using SystemDot.Messaging.Channels.Messages.Processing;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.processing
{
    [Subject("Message processing")]
    public class when_addressing_a_message : WithSubject<MessageAddresser>
    {
        static MessagePayload message;
        static MessagePayload processedPayload;
        
        Establish context = () =>
        {
            Subject.MessageProcessed += i => processedPayload = i;
            message = new MessagePayload();
        };

        Because of = () => Subject.InputMessage(message);

        It should_set_the_default_address_of_the_message = () =>
            processedPayload.GetToAddress().ShouldEqual(Address.Default);
    }
}