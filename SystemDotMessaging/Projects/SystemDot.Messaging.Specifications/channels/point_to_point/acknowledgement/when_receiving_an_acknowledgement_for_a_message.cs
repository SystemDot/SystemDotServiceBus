using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.acknowledgement
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_acknowledgement_for_a_message : WithMessageConfigurationSubject
    {
        static MessagePayload acknowledgement;
        static MessageRemovedFromCache @event;

        Establish context = () =>
        {
            Messenger.Register<MessageRemovedFromCache>(e => @event = e);
            
            var bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
                .Initialise();

            bus.Send(1);
             
            acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(Server.SentMessages.First().GetPersistenceId());
            acknowledgement.SetToAddress(Server.SentMessages.First().GetFromAddress());

            Server.ReceiveMessage(acknowledgement);
        };

        Because of = () => Server.ReceiveMessage(acknowledgement);

        It should_notify_that_the_message_was_removed_from_the_cache = () => 
            @event.ShouldMatch(e => e.MessageId == acknowledgement.GetAcknowledgementId().MessageId
                && e.Address == acknowledgement.GetAcknowledgementId().Address
                && e.UseType == acknowledgement.GetAcknowledgementId().UseType);
        
    }
}