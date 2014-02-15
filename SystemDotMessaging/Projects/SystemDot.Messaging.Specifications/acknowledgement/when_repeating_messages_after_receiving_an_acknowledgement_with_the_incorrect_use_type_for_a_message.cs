using System;
using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Simple;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.acknowledgement
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_messages_after_receiving_an_acknowledgement_with_the_incorrect_use_type_for_a_message 
        : WithMessageConfigurationSubject
    {
        static MessagePayload acknowledgement;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
                .Initialise();

            Bus.Send(1);

            MessagePayload message = GetServer().SentMessages.First();

            acknowledgement = new MessagePayload();
            
            acknowledgement.SetAcknowledgementId(
                new MessagePersistenceId(
                    message.Id, 
                    message.GetPersistenceId().Address, 
                    PersistenceUseType.PublisherSend));

            acknowledgement.SetToAddress(message.GetFromAddress());

            GetServer().ReceiveMessage(acknowledgement);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(4));
            GetServer().SentMessages.Clear();
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_have_repeated_the_message = () => GetServer().SentMessages.ShouldNotBeEmpty();
    }
}