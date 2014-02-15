using SystemDot.Messaging.Specifications.unitofwork;
using FluentAssertions;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.unitofwork_for_local_channels
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_local_message_with_a_unit_of_work_setup : WithMessageConfigurationSubject
    {
        static TestUnitOfWork unitOfWork;
        
        
        Establish context = () =>
        {
            unitOfWork = new TestUnitOfWork();
            ConfigureAndRegister<TestUnitOfWorkFactory>(new TestUnitOfWorkFactory(unitOfWork));

            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel()
                .WithUnitOfWork<TestUnitOfWorkFactory>()
                .Initialise();
        };

        Because of = () => Bus.SendDirect(new object());

        It should_begin_the_unit_of_work = () => unitOfWork.HasBegun().Should().BeTrue();
    }
}