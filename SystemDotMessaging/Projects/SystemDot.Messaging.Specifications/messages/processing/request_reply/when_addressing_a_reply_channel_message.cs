using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Specifications.messages.processing.request_reply
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

            The<ReplyChannelLookup>().SetCurrentChannel(toAddress);
            Subject = new ReplyChannelMessageAddresser(The<ReplyChannelLookup>(), fromAddress);
            Subject.MessageProcessed += i => processedMessage = i;

            message = new MessagePayload();
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processedMessage.ShouldBeTheSameAs(message);

        It should_address_the_reply_message_to_the_original_sender_ = () => processedMessage.GetToAddress().ShouldEqual(toAddress);
        
        It should_address_the_reply_message_from_the_reciever_ = () => processedMessage.GetFromAddress().ShouldEqual(fromAddress);
    }
}