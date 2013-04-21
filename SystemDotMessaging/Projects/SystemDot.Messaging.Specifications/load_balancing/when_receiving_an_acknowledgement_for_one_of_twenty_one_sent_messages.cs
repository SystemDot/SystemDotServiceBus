using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.load_balancing
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_acknowledgement_for_one_of_twenty_one_sent_messages : WithMessageConfigurationSubject
    {
        static List<int> messages;
        
        static MessagePayload acknowledgement;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress")
                .ForPointToPointSendingTo("ReceiverAddress")
                .Initialise();

            messages = Enumerable.Range(1, 21).ToList();

            messages.ForEach(m => Bus.Send(m));

            acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(Server.SentMessages.First().GetPersistenceId());
            acknowledgement.SetToAddress(Server.SentMessages.First().GetFromAddress());
        };

        Because of = () => Server.ReceiveMessage(acknowledgement);

        It should_send_the_twenty_first_message = () => Server.SentMessages.ElementAt(20).DeserialiseTo<int>().ShouldEqual(21);
    }
}