using System;
using System.Linq;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Storage.Changes;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject(SpecificationGroup.Description)]
    public class when_recieving_a_request_on_a_channel : WithMessageConfigurationSubject
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
                    .ForRequestReplyRecieving()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            message = 1;
            payload = new MessagePayload().MakeReceiveable(message, SenderAddress, ChannelName, PersistenceUseType.RequestSend);
        };

        Because of = () => MessageReciever.ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.HandledMessage.ShouldEqual(message);

        It should_send_an_acknowledgement_for_the_message = () =>
            MessageSender.SentMessages.ShouldContain(a => a.GetAcknowledgementId() == payload.GetSourcePersistenceId());

        It should_store_the_sender_address_for_the_reply_to_use = () =>
            Resolve<ReplyAddressLookup>().GetCurrentSenderAddress().ShouldEqual(BuildAddress(SenderAddress));

        It should_mark_the_message_with_the_time_the_message_is_sent = () =>
            Resolve<InMemoryChangeStore>()
                .GetAddedMessages(PersistenceUseType.RequestReceive, BuildAddress(SenderAddress))
                .First()
                .GetLastTimeSent().ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_message_with_the_amount_of_times_the_message_has_been_sent = () =>
           Resolve<InMemoryChangeStore>()
                .GetAddedMessages(PersistenceUseType.RequestReceive, BuildAddress(SenderAddress))
                .First()
                .GetAmountSent().ShouldEqual(1);
    }
}