using System.IoC.Example.Types;
using SystemDot.Ioc;

namespace System.IoC.Example
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
            catch (TypeNotRegisteredException ex)
            {
                Console.WriteLine(ex.Message);
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
