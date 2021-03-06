using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.sequencing_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_an_out_of_sequence_request_on_a_sequenced_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";

        static int message;
        static MessagePayload payload;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplyReceiving()
                    .Sequenced()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;

            payload = new MessagePayload().MakeReceivable(
                message, 
                SenderAddress, 
                ChannelName, 
                PersistenceUseType.RequestSend);

            payload.SetFirstSequence(1);
            payload.SetSequenceOriginSetOn(DateTime.Now);
            payload.SetSequence(2);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_not_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(0);
    }
}