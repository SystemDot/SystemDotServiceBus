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
        static EndpointAddress fromAddress;
        static EndpointAddress toAddress;

        Establish context = () =>
        {
            fromAddress = new EndpointAddress("TestFromAddress", "TestFromServer");
            toAddress = new EndpointAddress("TestToAddress", "TestToServer");

            Subject = new MessageAddresser(fromAddress, toAddress);

            Subject.MessageProcessed += i => processedPayload = i;
            message = new MessagePayload();
        };

        Because of = () => Subject.InputMessage(message);

        It should_set_the_from_address_of_the_message = () => processedPayload.GetFromAddress().ShouldEqual(fromAddress);

        It should_set_the_to_address_of_the_message = () => processedPayload.GetToAddress().ShouldEqual(toAddress);
    }
}