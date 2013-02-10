using System;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.serving
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_remote_server : WithRemoteServerConfigurationSubject
    {
        static TestSystemTime systemTime;

        Establish context = () =>
        {
            systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);
            ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());
        };

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport()
            .AsARemoteServer()
            .Initialise();

        It should_use_the_correct_long_poll_time = () => 
            systemTime.LastTimeSpanRequested.ShouldEqual(TimeSpan.FromSeconds(30));

        It should_listen_on_the_correct_url = () =>
            TestHttpServer.Instance.Url.ShouldEqual(String.Concat("http://", Environment.MachineName, ":8090/"));
    }
}