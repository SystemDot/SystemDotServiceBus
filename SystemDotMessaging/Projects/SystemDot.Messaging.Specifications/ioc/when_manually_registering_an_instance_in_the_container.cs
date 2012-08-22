using SystemDot.Messaging.Ioc;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_manually_registering_an_instance_in_the_container
    {
        static AnotherInheritingComponent created;

        Establish context = () =>
        {
            created = new AnotherInheritingComponent();
            IocContainer.RegisterInstance<IThirdTestComponent, ThirdTestComponent>();
        };

        Because of = () => IocContainer.RegisterInstance<IAnotherTestComponent>(() => created);

        It should_be_able_to_be_resolved = () => IocContainer.Resolve<IAnotherTestComponent>().ShouldBeTheSameAs(created);
    }
}