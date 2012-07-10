using SystemDot.Messaging.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages
{
    public class when_initialising_a_new_endpoint_address_with_an_address_and_a_proxy
    {
        static EndpointAddress address;

        Because of = () => address = "TestNode@TestServer.TestProxy";

        It should_set_the_specified_address = () => address.Address.ShouldEqual("TestNode@TestServer.TestProxy");

        It should_set_the_proxy_as_server_name = () => address.ServerName.ShouldEqual("TestProxy");
    }
}