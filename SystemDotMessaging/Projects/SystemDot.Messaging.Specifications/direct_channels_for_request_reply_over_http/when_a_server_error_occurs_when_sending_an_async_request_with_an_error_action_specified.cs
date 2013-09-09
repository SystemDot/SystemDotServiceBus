using System;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply_over_http
{
    [Subject(SpecificationGroup.Description)]
    public class when_a_server_error_occurs_when_sending_an_async_request_with_an_error_action_specified
        : WithHttpServerConfigurationSubject
    {
        const long Message = 1;
        static Exception expectedException;
        static Exception exception;
        static TestTaskStarter taskStarter;

        Establish context = () =>
        {
            taskStarter = new TestTaskStarter(1);
            ConfigureAndRegister<ITaskStarter>(taskStarter);

            expectedException = new Exception();
            WebRequestor.ThrowExceptionOnPut(expectedException);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer("SenderServer")
                .OpenDirectChannel("SenderChannel")
                .ForRequestReplySendingTo("ReceiverChannel@ReceiverServer")
                .Initialise();
        };

        Because of = () => Bus.SendDirectAsync(Message, e => exception = e);

        It should_send_asynchronously = () => taskStarter.InvocationCount.ShouldEqual(1);

        It should_run_the_server_error_action = () => exception.ShouldEqual(expectedException);

        It should_switch_to_the_main_thread_to_handle_the_error = () => MainThreadMarshaller.WasRunThrough.ShouldBeTrue();
    }
}