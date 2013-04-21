using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_on_a_durable_channel : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";
        
        

        Establish context = () =>
        {    
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress).ForPointToPointSendingTo(ReceiverAddress)
                .WithDurability()
                .Initialise();
        };

        Because of = () => Bus.Send(1);

        It should_persist_the_message = () =>
            Resolve<IChangeStore>()
                .GetSendMessages(PersistenceUseType.PointToPointSend, BuildAddress(SenderAddress))
                .ShouldNotBeEmpty();
    }
}