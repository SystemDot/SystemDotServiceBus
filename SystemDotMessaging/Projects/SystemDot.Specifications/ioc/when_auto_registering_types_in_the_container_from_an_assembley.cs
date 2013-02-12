﻿using System.Collections.Generic;
using SystemDot.Ioc;
using SystemDot.Specifications.ioc.TestTypes;
using Machine.Fakes;
using Machine.Specifications;
using Machine.Specifications.Model;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_auto_registering_types_in_the_container_from_an_assembley : WithSubject<IocContainer>
    {
        Because of = () => Subject.RegisterFromAssemblyOf<when_auto_registering_types_in_the_container_from_an_assembley>();

        It should_auto_register_a_concrete_type_by_the_concrete_type = () =>
            Subject.Resolve<TestTypeNotImplementingAnInterface>().ShouldBeOfType<TestTypeNotImplementingAnInterface>();

        It should_auto_register_a_concrete_type_implementing_an_interface_by_the_interface = () =>
            Subject.Resolve<ITestInterface1>().ShouldBeOfType<TestTypeImplementingAnInterface>();

        It should_not_auto_register_an_interface_without_concrete_type_implementing_it = () =>
            Catch.Exception(() => Subject.Resolve<ITestInterfaceWithNoConcreteImplementation>())
            .ShouldBeOfType<KeyNotFoundException>();

        It should_not_auto_register_an_abstract_concrete_type = () =>
            Catch.Exception(() => Subject.Resolve<TestAbstractConcreteType>())
            .ShouldBeOfType<KeyNotFoundException>();

        It should_auto_register_a_derived_concrete_type_against_its_interface = () =>
            Subject.Resolve<ITestInterfaceOnDerivedConcreteType>().ShouldBeOfType<TestDerivedConcreteType>();
    }
}
