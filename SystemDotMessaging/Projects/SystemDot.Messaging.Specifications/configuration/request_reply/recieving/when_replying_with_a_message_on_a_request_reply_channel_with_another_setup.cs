using System;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Specifications.configuration.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")]
    public class when_replying_with_a_message_on_a_request_reply_channel_with_another_setup : WithNoRepeaterMessageConfigurationSubject
    {
        const string ChannelName = "Test1";
        
        static IBus bus;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForRequestReplyRecieving()
                .OpenChannel("Test2").ForRequestReplyRecieving()
                .Initialise();

            MessageReciever.RecieveMessage(new MessagePayload().MakeReceiveable(1, "TestSender", ChannelName, PersistenceUseType.RequestSend));
        };

        Because of = () => bus.Reply(1);

        It should_only_send_the_message_to_the_correct_channel = () =>
            MessageSender.SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(1);
    }
}