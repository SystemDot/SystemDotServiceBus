using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing.RequestReply;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Specifications.messages.processing.request_reply
{
    [Subject("Message processing")]
    public class when_passing_messages_through_a_reply_channel_selector : WithSubject<ReplyChannelSelector>
    {
        static EndpointAddress senderAddress;
        static EndpointAddress recieverAddress;
        static MessagePayload message;
        static MessagePayload processedMessage;
        
        Establish context = () =>
        {
            senderAddress = new EndpointAddress("SenderChannel", "SenderServer");
            recieverAddress = new EndpointAddress("RecieverChannel", "RecieverServer");
            
            Subject = new ReplyChannelSelector(The<ReplyAddressLookup>());
            Subject.MessageProcessed += i => processedMessage = i;

            message = new MessagePayload();
            message.SetFromAddress(senderAddress);
            message.SetToAddress(recieverAddress);
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processedMessage.ShouldBeTheSameAs(message);

        It should_have_set_the_current_sender_address_for_lookup = () => 
            The<ReplyAddressLookup>().GetCurrentSenderAddress().ShouldBeTheSameAs(senderAddress);
        
        It should_have_set_the_current_reciever_address_for_lookup = () =>
            The<ReplyAddressLookup>().GetCurrentRecieverAddress().ShouldBeTheSameAs(recieverAddress);
    }
}