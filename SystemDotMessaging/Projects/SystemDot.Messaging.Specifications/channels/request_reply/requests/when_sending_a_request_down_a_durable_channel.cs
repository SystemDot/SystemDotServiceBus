using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.requests
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_down_a_durable_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";
        static IBus bus;
        static int message;
        
        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForRequestReplySendingTo(RecieverAddress).WithDurability()
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_persist_the_message = () =>
            Resolve<IChangeStore>()
                .GetSendMessages(PersistenceUseType.RequestSend, BuildAddress(ChannelName))
                .ShouldNotBeEmpty();
    }
}