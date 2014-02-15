using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Handling.Actions;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Simple;
using SystemDot.Messaging.Storage;
using Machine.Specifications;
using FluentAssertions;

namespace SystemDot.Messaging.Specifications.acknowledgement
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_acknowledgement_for_a_message : WithMessageConfigurationSubject
    {
        static MessagePayload acknowledgement;
        static MessageRemovedFromCache @event;
        static ActionSubscriptionToken<MessageRemovedFromCache> token;

        Establish context = () =>
        {
            
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
                .Initialise();

            Bus.Send(1);

            token = Messenger.RegisterHandler<MessageRemovedFromCache>(e => @event = e);
            
            acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(GetServer().SentMessages.First().GetPersistenceId());
            acknowledgement.SetToAddress(GetServer().SentMessages.First().GetFromAddress());

            GetServer().ReceiveMessage(acknowledgement);
        };

        Because of = () => GetServer().ReceiveMessage(acknowledgement);

        It should_notify_that_the_message_was_removed_from_the_cache = () =>
            @event.Should().Match<MessageRemovedFromCache>(e => e.MessageId == acknowledgement.GetAcknowledgementId().MessageId
                && e.Address == acknowledgement.GetAcknowledgementId().Address
                && e.UseType == acknowledgement.GetAcknowledgementId().UseType);
        
    }
}