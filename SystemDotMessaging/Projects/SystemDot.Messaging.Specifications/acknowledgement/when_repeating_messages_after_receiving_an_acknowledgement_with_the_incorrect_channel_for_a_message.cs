using System;
using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.acknowledgement
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_messages_after_receiving_an_acknowledgement_with_the_incorrect_channel_for_a_message
        : WithMessageConfigurationSubject
    {
        static MessagePayload acknowledgement;
        static TestSystemTime systemTime;

        Establish context = () =>
        {
            systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
                .Initialise();

            Bus.Send(1);

            MessagePayload message = GetServer().SentMessages.First();

            acknowledgement = new MessagePayload();

            acknowledgement.SetAcknowledgementId(
                new MessagePersistenceId(
                    message.Id,
                    BuildAddress("Test", "Test"),
                    message.GetPersistenceId().UseType));

            acknowledgement.SetToAddress(message.GetFromAddress());

            GetServer().ReceiveMessage(acknowledgement);

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));
            GetServer().SentMessages.Clear();
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_have_repeated_the_message = () => GetServer().SentMessages.ShouldNotBeEmpty();
    }
}