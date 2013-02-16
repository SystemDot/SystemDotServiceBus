using SystemDot.Http;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Specifications.channels;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_remote_client_and_the_long_poll_request_fails : WithConfigurationSubject
    {
        static TestTaskStarter taskStarter;
        static FailingWebRequestor requestor;

        Establish context = () =>
        {
            requestor = new FailingWebRequestor();
            ConfigureAndRegister<IWebRequestor>(requestor);

            taskStarter = new TestTaskStarter(2);
            ConfigureAndRegister<ITaskStarter>(taskStarter);
        };

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport()
            .AsARemoteClient("RemoteClientInstance")
            .UsingProxy(MessageServer.Local("RemoteProxyInstance"))
            .OpenChannel("ReceiverName")
            .ForPointToPointReceiving()
            .Initialise();

        It should_not_fail_and_should_continue_polling = () => requestor.RequestCount.ShouldEqual(2);
    }
}