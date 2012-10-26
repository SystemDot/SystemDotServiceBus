using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.RequestReply;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply
{
    [Subject("Message processing")]
    public class when_addressing_a_reply_channel_message : WithSubject<ReplyChannelMessageAddresser>
    {
        static EndpointAddress fromAddress;
        static EndpointAddress toAddress;
        static MessagePayload message;
        static MessagePayload processedMessage;
        
        Establish context = () =>
        {
            fromAddress = new EndpointAddress("FromChannel", "FromServer");
            toAddress = new EndpointAddress("ToChannel", "ToServer");

            The<ReplyAddressLookup>().SetCurrentSenderAddress(toAddress);
            Subject = new ReplyChannelMessageAddresser(The<ReplyAddressLookup>(), fromAddress);
            Subject.MessageProcessed += i => processedMessage = i;

            message = new MessagePayload();
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processedMessage.ShouldBeTheSameAs(message);

        It should_address_the_reply_message_to_the_original_sender_ = () => processedMessage.GetToAddress().ShouldEqual(toAddress);
        
        It should_address_the_reply_message_from_the_reciever_ = () => processedMessage.GetFromAddress().ShouldEqual(fromAddress);
    }
}