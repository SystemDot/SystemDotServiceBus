using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using Machine.Specifications;
using SystemDot.Messaging.Channels.Sequencing;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")]
    public class when_replying_with_a_message_on_a_durable_request_reply_channel : WithNoRepeaterMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        static IBus bus;
        static int message;
        
        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()                
                .OpenChannel(ChannelName).ForRequestReplyRecieving().WithDurability()
                .Initialise();

            MessagePayload request = new MessagePayload().MakeReceiveable(1, SenderChannelName, ChannelName, PersistenceUseType.RequestSend);
            request.SetSequence(1);
            MessageReciever.RecieveMessage(request);

            message = 1;
        };

        Because of = () => bus.Reply(message);

        It should_persist_the_message = () =>
            Resolve<IChangeStore>()
                .GetMessages(PersistenceUseType.ReplySend, BuildAddress(SenderChannelName))
                .ShouldNotBeEmpty();
    }
}