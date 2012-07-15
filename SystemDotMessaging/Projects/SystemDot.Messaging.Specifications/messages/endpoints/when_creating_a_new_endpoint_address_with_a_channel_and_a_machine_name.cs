using SystemDot.Messaging.Messages;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.endpoints
{
    [Subject("Endpoints")]
    public class when_creating_a_new_endpoint_address_with_a_channel_and_a_machine_name
    {
        const string Channel = "TestChannel";
        const string Machine = "TestMachine";
        static EndpointAddress address;

        Because of = () => address = new EndpointAddress(Channel, Machine);

        It should_set_the_specified_address = () => address.Address.ShouldEqual("TestChannel@TestMachine.TestMachine");

        It should_set_the_channel = () => address.Channel.ShouldEqual(Channel);

        It should_set_the_specified_server_name = () => address.ServerName.ShouldEqual(Machine);
    }
}