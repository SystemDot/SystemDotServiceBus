using System;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.serving
{
    [Subject(SpecificationGroup.Description)]
    public class when_handling_request_that_fails_deserialisation : WithRemoteServerConfigurationSubject
    {
        static Exception exception;
        
        Establish context = () =>
        {
            ConfigureAndRegister<ISerialiser>(new FailingSerialiser());
            ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteServer()
                .Initialise();
        };

        Because of = () => exception = Catch.Exception(() => SendObjectsToRemoteServer(new object()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}