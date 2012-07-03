using SystemDot.Messaging.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages
{
    public class when_initialising_a_new_endpoint_address_with_only_a_node_name
    {
        static EndpointAddress address;

        Because of = () => address = "TestNode";

        It should_set_the_specified_node_name = () => address.NodeName.ShouldEqual("TestNode");

        It should_not_set_the_server_name = () => address.ServerName.ShouldBeEmpty();
    }
}