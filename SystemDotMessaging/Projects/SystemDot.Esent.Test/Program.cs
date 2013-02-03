using System;
using System.Linq;
using SystemDot.Files;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;

namespace SystemDot.Esent.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var store = new EsentChangeStore(new FileSystem(), new PlatformAgnosticSerialiser());
            store.Initialise("Messaging");

            foreach (var c in store.GetChanges("Test"))
            {
                Console.WriteLine(c.As<TestChange>().Number);    
            }

            foreach (var c in store.GetChanges("Test1"))
            {
                Console.WriteLine(c.As<TestChange>().Number);
            }

            Console.ReadKey();

            for (int i = 0; i < 1500; i++)
            {
                store.StoreChange("Test", new TestChange { Number = i });
                Console.WriteLine(i);                
            }

            for (int i = 0; i < 1500; i++)
            {
                store.StoreChange("Test1", new TestChange { Number = i });
                Console.WriteLine(i);
            }

            var changes = store.GetChanges("Test");
            Console.WriteLine(changes.Count());

            changes = store.GetChanges("Test1");
            Console.WriteLine(changes.Count());
        }
    }

    class TestChange : Change
    {
        public int Number { get; set; }
    }
}
