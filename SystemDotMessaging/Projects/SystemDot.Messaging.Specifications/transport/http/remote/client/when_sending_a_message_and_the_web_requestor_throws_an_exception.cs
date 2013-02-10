using System;
using SystemDot.Http;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Specifications.configuration;
using SystemDot.Messaging.Transport.Http.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_and_the_web_requestor_throws_an_exception : WithConfigurationSubject
    {
        static Exception exception;
        static IBus bus;

        Establish context = () =>
        {
            ConfigureAndRegister<IWebRequestor>(new FailingWebRequestor());
            bus = Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteClientOf(MessageServer.Local())
                .OpenChannel("TestSender")
                .ForPointToPointSendingTo("TestReceiver")
                .Initialise();
        };

        Because of = () => exception = Catch.Exception(() => bus.Send(1));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}
