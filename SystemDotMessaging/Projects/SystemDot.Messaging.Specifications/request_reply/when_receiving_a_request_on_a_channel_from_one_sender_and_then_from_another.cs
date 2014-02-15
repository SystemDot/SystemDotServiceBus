using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_on_a_channel_from_one_sender_and_then_from_another 
        : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        const string Sender1Address = "Sender1Address";
        const string Sender2Address = "Sender2Address";
        const Int64 Message = 1;

        static MessagePayload payload1;
        static MessagePayload payload2;
        static TestMessageHandler<Int64> handler;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForRequestReplyReceiving()
                .Initialise();

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);

            payload1 = new MessagePayload().MakeSequencedReceivable(
                Message,
                Sender1Address,
                ReceiverAddress,
                PersistenceUseType.RequestSend);

            GetServer().ReceiveMessage(payload1);

            payload2 = new MessagePayload().MakeSequencedReceivable(
                Message,
                Sender2Address,
                ReceiverAddress,
                PersistenceUseType.RequestSend);
        };

        Because of = () => GetServer().ReceiveMessage(payload2);

        It should_only_handle_the_requests_for_each_sender_once = () => 
            handler.HandledMessages.Count.ShouldBeEquivalentTo(2);
    }
}