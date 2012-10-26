using System;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.acknowledgement
{
    public class when_acknowledging_a_message : WithSubject<MessageAcknowledger>
    {
        static MessagePayload message;
        static MessagePayload processedMessage;
        static TestMessageSender sender;

        Establish context = () =>
        {
            sender = new TestMessageSender();
            Configure<IMessageSender>(sender);

            message = new MessagePayload();
            message.SetFromAddress(new EndpointAddress("Channel", "Server"));
            message.SetPersistenceId(message.GetFromAddress(), PersistenceUseType.ReplyReceive);           
            Subject.MessageProcessed += m => processedMessage = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_output_the_message = () => processedMessage.ShouldBeTheSameAs(message);

        It should_send_a_acknowledgement_for_the_message_for_the_correct_message_id = () =>
            sender.SentMessages.Single().GetAcknowledgementId().ShouldEqual(message.GetPersistenceId());

        It should_send_a_acknowledgement_for_the_message_to_the_message_from_address = () =>
            sender.SentMessages.Single().GetToAddress().ShouldEqual(message.GetFromAddress());
    }
}