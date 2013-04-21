using System;
using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.Specifications;
using Machine.Specifications;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.Specifications.repeating.escalating.http
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_and_four_seconds_have_passed : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";
        const string ServerInstance = "ServerInstance";

        static TestWebRequestor webRequestor;
        
        Establish context = () =>
        {
            ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

            webRequestor = new TestWebRequestor(
                new PlatformAgnosticSerialiser(),
                new FixedPortAddress(Environment.MachineName, ServerInstance));

            ConfigureAndRegister<IWebRequestor>(webRequestor);

            var systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer(ServerInstance)
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(SenderChannelName)
                .Initialise();

            Bus.Send(1);

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => webRequestor.RequestsMade.Count.ShouldEqual(2);
    }
}