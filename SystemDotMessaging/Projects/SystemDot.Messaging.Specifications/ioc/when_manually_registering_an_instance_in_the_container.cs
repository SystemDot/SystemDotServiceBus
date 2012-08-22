﻿using SystemDot.Messaging.Ioc;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_manually_registering_an_instance_in_the_container : WithSubject<IocContainer>
    {
        static AnotherInheritingComponent created;

        Establish context = () =>
        {
            created = new AnotherInheritingComponent();
            Subject.RegisterInstance<IThirdTestComponent, ThirdTestComponent>();
        };

        Because of = () => Subject.RegisterInstance<IAnotherTestComponent>(() => created);

        It should_be_able_to_be_resolved = () => Subject.Resolve<IAnotherTestComponent>().ShouldBeTheSameAs(created);
    }
}