using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels
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