using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.load_balancing
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_acknowledgement_for_one_of_twenty_one_sent_messages_but_on_a_different_channel_use_type : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string RecieverAddress = "ReceiverAddress";

        static List<int> messages;
        
        static MessagePayload acknowledgement;

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress)
                .ForPointToPointSendingTo(RecieverAddress)
                .OpenChannel(SenderAddress)
                .ForRequestReplySendingTo(RecieverAddress)
                .OnlyForMessages().WithNamePattern("NotInt")
                .Initialise();

            messages = Enumerable.Range(1, 21).ToList();
            messages.ForEach(m => Bus.Send(m));

            acknowledgement = new MessagePayload();

            acknowledgement.SetAcknowledgementId(new MessagePersistenceId(
                GetServer().SentMessages.First().Id, 
                BuildAddress(SenderAddress), 
                PersistenceUseType.RequestSend));

            acknowledgement.SetToAddress(GetServer().SentMessages.First().GetFromAddress());
        };

        Because of = () => GetServer().ReceiveMessage(acknowledgement);

        It should_not_send_the_twenty_first_message = () => GetServer().SentMessages.Count.ShouldEqual(20);
    }
}