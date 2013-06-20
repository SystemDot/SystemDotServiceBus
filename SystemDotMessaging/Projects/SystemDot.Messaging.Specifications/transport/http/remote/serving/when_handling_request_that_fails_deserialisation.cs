using System;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.serving
{
    [Subject(SpecificationGroup.Description)]
    public class when_handling_request_that_fails_deserialisation : WithServerConfigurationSubject
    {
        static Exception exception;
        
        Establish context = () =>
        {
            ConfigureAndRegister<ISerialiser>(new FailingSerialiser());

            Messaging.Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAProxy("RemoteServerName")
                .Initialise();
        };

        Because of = () => exception = Catch.Exception(() => SendObjectsToServer(new object()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}