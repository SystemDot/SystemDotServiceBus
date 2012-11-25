using System;
using System.Linq;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Storage.Esent.Test
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

            Console.ReadKey();

            for (int i = 0; i < 5000; i++)
            {
                Guid id = store.StoreChange("Test", new TestChange { Number = i });
                var change = store.GetChange(id).As<TestChange>();
                Console.WriteLine(change.Number);                
            }

            var changes = store.GetChanges("Test");
            Console.WriteLine(changes.Count());
        }
    }

    class TestChange : Change
    {
        public int Number { get; set; }
    }
}
