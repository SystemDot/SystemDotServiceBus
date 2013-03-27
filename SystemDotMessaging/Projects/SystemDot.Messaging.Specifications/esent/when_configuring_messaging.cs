using SystemDot.Esent;
using SystemDot.Storage.Changes;
using Machine.Specifications;
using SystemDot.Messaging.Transport.InProcess.Configuration;

namespace SystemDot.Messaging.Specifications.esent
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_messaging_with_file_persistence : WithConfigurationSubject
    {
        Because of = () => Configuration.Configure.Messaging()
            .UsingFilePersistence()
            .UsingInProcessTransport();

        It should_have_registered_the_esent_change_store = () => 
            Resolve<IChangeStore>().ShouldBeOfType<EsentChangeStore>();  
    }
}