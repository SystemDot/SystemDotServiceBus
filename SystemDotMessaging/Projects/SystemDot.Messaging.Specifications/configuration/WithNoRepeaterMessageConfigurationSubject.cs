using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    public class WithNoRepeaterMessageConfigurationSubject : WithMessageConfigurationSubject
    {
        Establish context = () =>
        {
            ConfigureAndRegister(TestTaskStarter.Umlimited());
            ConfigureAndRegister<ITaskRepeater>();
        };
    }
}