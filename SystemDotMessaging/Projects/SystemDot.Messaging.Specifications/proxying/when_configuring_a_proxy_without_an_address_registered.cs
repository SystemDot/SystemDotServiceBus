using System;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.proxying
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_proxy_without_an_address_registered : WithHttpServerConfigurationSubject
    {
        const string RemoteServerName = "RemoteServerName";

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport()
            .AsAProxyFor(RemoteServerName)
            .Initialise();

        It should_use_the_correct_long_poll_time = () => SystemTime.LastTimeSpanRequested.ShouldEqual(TimeSpan.FromSeconds(30));

        It should_listen_on_the_correct_url = () =>
            TestHttpServer.Instance.Url.ShouldEqual(String.Concat("http://", System.Environment.MachineName, "/", RemoteServerName + "/"));
    }
}