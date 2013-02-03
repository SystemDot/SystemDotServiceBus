using System;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_without_configuring_a_publish_channel : WithMessageConfigurationSubject
    {
        static IBus bus;
        static Exception exception;
        
        Establish context = () =>
        {    
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenLocalChannel()
                .Initialise();
        };

        Because of = () => exception = Catch.Exception(() => bus.Publish(1));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}