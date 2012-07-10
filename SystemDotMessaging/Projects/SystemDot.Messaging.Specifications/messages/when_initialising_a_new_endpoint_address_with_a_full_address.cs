using SystemDot.Messaging.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages
{
    public class when_initialising_a_new_endpoint_address_with_a_full_address
    {
        static EndpointAddress address;

        Because of = () => address = "TestNode@TestServer";

        It should_set_the_specified_address = () => address.Address.ShouldEqual("TestNode@TestServer");

        It should_set_the_specified_server_name = () => address.ServerName.ShouldEqual("TestServer");
    }
}