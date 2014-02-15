using System;
using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.acknowledgement
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_messages_after_receiving_an_acknowledgement_for_a_message : WithMessageConfigurationSubject
    {
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
                .Initialise();

            Bus.Send(1);
             
            var acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(GetServer().SentMessages.First().GetPersistenceId());
            acknowledgement.SetToAddress(GetServer().SentMessages.First().GetFromAddress());

            GetServer().ReceiveMessage(acknowledgement);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(4));
            GetServer().SentMessages.Clear();
        };

        Because of = () => Resolve<ITaskRepeater>().Start();

        It should_not_have_repeated_the_message = () => GetServer().SentMessages.ShouldBeEmpty();
    }
}