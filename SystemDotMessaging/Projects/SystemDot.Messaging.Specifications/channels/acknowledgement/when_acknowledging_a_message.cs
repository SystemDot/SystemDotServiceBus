using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.acknowledgement
{
    public class when_acknowledging_a_message : WithSubject<MessageAcknowledger>
    {
        static MessagePayload message;
        static MessagePayload processedMessage;
        static MessagePayload processedAcknowedgement;
        
        Establish context = () =>
        {
            Configure(new AcknowledgementSender());
            The<AcknowledgementSender>().MessageProcessed += m => processedAcknowedgement = m;

            message = new MessagePayload();
            message.SetFromAddress(new EndpointAddress("GetChannel", "Server"));
            message.SetSourcePersistenceId(new MessagePersistenceId(message.Id, message.GetFromAddress(), PersistenceUseType.ReplyReceive));           
            Subject.MessageProcessed += m => processedMessage = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_output_the_message = () => processedMessage.ShouldBeTheSameAs(message);

        It should_send_a_acknowledgement_for_the_message_for_the_correct_message_id = () =>
            processedAcknowedgement.GetAcknowledgementId().ShouldEqual(message.GetSourcePersistenceId());

        It should_send_a_acknowledgement_for_the_message_to_the_message_from_address = () =>
            processedAcknowedgement.GetToAddress().ShouldEqual(message.GetFromAddress());
    }

    
}