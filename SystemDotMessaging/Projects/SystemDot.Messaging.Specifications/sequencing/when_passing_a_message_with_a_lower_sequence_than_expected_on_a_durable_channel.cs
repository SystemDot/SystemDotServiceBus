using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_passing_a_message_with_a_lower_sequence_than_expected_on_a_durable_channel
        : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";

        static MessagePayload messagePayload;
        static TestMessageHandler<int> handler;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                 .UsingInProcessTransport()
                 .OpenChannel(ReceiverAddress).ForPointToPointReceiving().WithDurability()
                 .Initialise();

            messagePayload = new MessagePayload()
                .MakeReceivable(1, "SenderAddress", ReceiverAddress, PersistenceUseType.PointToPointSend);

            messagePayload.SetSequence(1);
            messagePayload.SetFirstSequence(2);
            messagePayload.SetSequenceOriginSetOn(DateTime.Today);

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload);

        It should_not_pass_the_message_through = () => handler.HandledMessages.ShouldBeEmpty();
    }
}