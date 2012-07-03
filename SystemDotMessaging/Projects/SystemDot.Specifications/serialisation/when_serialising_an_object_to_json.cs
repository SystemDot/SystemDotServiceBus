using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Specifications.serialisation
{
    [Subject("Serialisation")]
    public class when_serialising_an_object_to_json
    {
        static BinarySerialiser serialiser;
        static string item;
        static byte[] serialisedItem;
        
        Establish context = () =>
        {
            item = "Test";
            serialiser = new BinarySerialiser(new BinaryFormatter());
        };

        Because of = () => serialisedItem = serialiser.Serialise(item);

        It should_deserialise_into_an_equivelent_object = () => serialiser.Deserialise(serialisedItem).ShouldEqual(item);
    }
}