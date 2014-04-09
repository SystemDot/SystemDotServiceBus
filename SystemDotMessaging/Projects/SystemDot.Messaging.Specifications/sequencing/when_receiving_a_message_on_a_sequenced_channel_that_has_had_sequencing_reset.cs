using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_on_a_sequenced_channel_that_has_had_sequencing_reset : WithMessageConfigurationSubject
    {
        const Int64 Message = 1;
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderAddress = "SenderAddress";

        static TestMessageHandler<Int64> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForPointToPointReceiving().Sequenced()
                .Initialise();

            messagePayload = new MessagePayload()
                .MakeReceivable(Message, SenderAddress, ReceiverAddress, PersistenceUseType.PointToPointSend);
            messagePayload.SetSequence(2);
            messagePayload.SetSequenceOriginSetOn(DateTime.Today);
            messagePayload.SetFirstSequence(2);

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload);

        It should_pass_the_message_through = () => handler.HandledMessages.Should().Contain(Message);
    }
}