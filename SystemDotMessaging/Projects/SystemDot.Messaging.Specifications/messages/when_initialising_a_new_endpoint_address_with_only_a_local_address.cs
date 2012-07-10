using SystemDot.Messaging.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages
{
    public class when_initialising_a_new_endpoint_address_with_only_a_local_address
    {
        static EndpointAddress address;

        Because of = () => address = "TestAddress";

        It should_set_the_specified_node_name = () => address.Address.ShouldEqual("TestAddress");

        It should_not_set_the_server_name = () => address.ServerName.ShouldBeEmpty();
    }
}