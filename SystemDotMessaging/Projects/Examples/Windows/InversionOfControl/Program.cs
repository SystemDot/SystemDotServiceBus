using System;
using SystemDot.Ioc;
using InversionOfControl.Types;

namespace InversionOfControl
{
    class Program
    {
        static void Main(string[] args)
        {
            var ioc = new IocContainer();
            CustomRegistrations(ioc);
            AutoRegistrations(ioc);

            Console.WriteLine(ioc.Resolve<ISomething>().Say());
            Console.WriteLine(ioc.Resolve<Something>().Say());
            Console.WriteLine(ioc.Resolve<ISomethingOrOther>().Say());
            Console.WriteLine(ioc.Resolve<IInterfaceForBaseType>().Say());
            Console.WriteLine(ioc.Resolve<IInterfaceForDerivedType>().Say());
            Console.WriteLine(ioc.Resolve<ICustomInterface>().Say());

            try
            {
                ioc.Resolve<Object>();
            }
            catch (TypeNotRegisteredException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }

        static void CustomRegistrations(IocContainer ioc)
        {
            ioc.RegisterInstance<ICustomInterface, CustomType>();
        }

        static void AutoRegistrations(IocContainer ioc)
        {
            ioc.RegisterFromAssemblyOf<Program>();
        }
    }
}
