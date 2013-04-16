using SystemDot.Ioc;
using SystemDot.Messaging.Specifications.handling.Fakes;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.handling
{
    [Subject("Handling")]
    public class when_initialising_with_handlers_auto_registered : WithConfigurationSubject
    {
        static IocContainer container;
        static bool wasAnythingResolvedFromIoc;

        Establish context = () =>
        {
            container = new IocContainer();
            Register(container, new FirstHandlerOfMessage1());
            Register(container, new SecondHandlerOfMessage1());
            Register(container, new FirstHandlerOfMessage2());
            Register(container, new SecondHandlerOfMessage2());
        };

        Because of = () => 
            Configuration.Configure.Messaging()
            .RegisterHandlersFromAssemblyOf<when_initialising_with_handlers_auto_registered>()
            .BasedOn<IHandleMessage>()
            .ResolveBy(type =>
                {
                    wasAnythingResolvedFromIoc = true;
                    return container.Resolve(type);
                })
            .UsingInProcessTransport()
            .OpenLocalChannel()
            .Initialise();

        It should_not_resolve_any_message_handlers = () => wasAnythingResolvedFromIoc.ShouldBeFalse();
    }
}