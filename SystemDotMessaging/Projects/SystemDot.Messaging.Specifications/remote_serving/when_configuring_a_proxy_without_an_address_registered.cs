using System;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.remote_serving
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_proxy_without_an_address_registered : WithHttpServerConfigurationSubject
    {
        const string RemoteServerName = "RemoteServerName";
        static TestSystemTime systemTime;

        Establish context = () =>
        {
            systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);
        };

        Because of = () => Messaging.Configuration.Configure.Messaging()
            .UsingHttpTransport()
            .AsAProxyFor(RemoteServerName)
            .Initialise();

        It should_use_the_correct_long_poll_time = () => 
            systemTime.LastTimeSpanRequested.ShouldEqual(TimeSpan.FromSeconds(30));

        It should_listen_on_the_correct_url = () =>
            TestHttpServer.Instance
                .Url.ShouldEqual(String.Concat("http://", Environment.MachineName, "/", RemoteServerName + "/"));
    }
}