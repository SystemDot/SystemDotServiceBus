using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels
{
    [Subject("Configuration")] 
    public class when_logging_a_message_with_a_logging_mechanism_configured : WithConfigurationSubject
    {
        static Exception exception;
        static ILoggingMechanism toLogWith;    

        Establish context = () =>
        {
            exception = new Exception();
            IocContainerLocator.SetContainer(new IocContainer());
            toLogWith = An<ILoggingMechanism>();
            Configuration.Configure.Messaging().LoggingWith(toLogWith);
        };

        Because of = () => Logger.Error(exception);

        It should_log_the_message = () => toLogWith.WasToldTo(l => l.Error(exception));
    }
}