using SystemDot.Logging;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    [Subject("Configuration")] 
    public class when_logging_a_message_with_a_logging_mechanism_configured : WithConfiguationSubject
    {
        const string MessageToLog = "Test";
        static ILoggingMechanism toLogWith;    

        Establish context = () =>
        {
            toLogWith = An<ILoggingMechanism>();

            Configuration.Configure.Messaging().LoggingWith(toLogWith);
        };

        Because of = () => Logger.Error(MessageToLog);

        It should_log_the_message = () => toLogWith.WasToldTo(l => l.Error(MessageToLog));
    }
}