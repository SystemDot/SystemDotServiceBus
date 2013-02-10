using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.configuration.publishing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.replies
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_on_a_channel_with_another_setup : WithMessageConfigurationSubject
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

            MessageServer.ReceiveMessage(new MessagePayload().MakeReceiveable(
                1, 
                "TestSender", 
                ChannelName, 
                PersistenceUseType.RequestSend));
        };

        Because of = () => bus.Reply(1);

        It should_only_send_the_message_to_the_correct_channel = () =>
            MessageServer.SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(1);
    }
}