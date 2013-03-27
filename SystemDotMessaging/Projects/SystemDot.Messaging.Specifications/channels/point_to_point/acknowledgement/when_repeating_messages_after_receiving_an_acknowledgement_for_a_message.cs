using System;
using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.acknowledgement
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_messages_after_receiving_an_acknowledgement_for_a_message : WithMessageConfigurationSubject
    {
        Establish context = () =>
        {
            var systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            var bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
                .Initialise();

            bus.Send(1);
             
            var acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(Server.SentMessages.First().GetPersistenceId());
            acknowledgement.SetToAddress(Server.SentMessages.First().GetFromAddress());

            Server.ReceiveMessage(acknowledgement);

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));
            Server.SentMessages.Clear();
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_not_have_repeated_the_message = () => Server.SentMessages.ShouldBeEmpty();
    }
}