using SystemDot.Messaging.Channels;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.endpoints
{
    [Subject("Endpoints")]
    public class when_building_a_new_endpoint_address_with_a_specified_channel_machine_and_server
        : WithSubject<EndpointAddressBuilder>
    {
        const string TestAddress = "TestChannel@TestMachine";
        const string TestServerName = "TestServer";

        static EndpointAddress address;

        Because of = () => address = Subject.Build(TestAddress + "." + TestServerName, "LocalServer");

        It should_set_the_specified_address = () => address.Channel.ShouldEqual(TestAddress);

        It should_set_the_specified_server_name = () => address.ServerName.ShouldEqual(TestServerName);
    }
}