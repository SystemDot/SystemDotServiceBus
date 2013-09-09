using System.Linq;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_an_async_request : WithMessageConfigurationSubject
    {
        const long Message = 1;

        static TestTaskStarter taskStarter;

        Establish context = () =>
        {
            taskStarter = new TestTaskStarter(1);
            ConfigureAndRegister<ITaskStarter>(taskStarter);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel("Sender").ForRequestReplySendingTo("Receiver")
                .Initialise();
        };

        Because of = () => Bus.SendDirectAsync(Message);

        It should_send_asynchronously = () => taskStarter.InvocationCount.ShouldEqual(1);

        It should_send_the_message_with_the_correct_payload = () =>
            GetServer().SentMessages.Single().DeserialiseTo<long>().ShouldEqual(Message);
    }
}