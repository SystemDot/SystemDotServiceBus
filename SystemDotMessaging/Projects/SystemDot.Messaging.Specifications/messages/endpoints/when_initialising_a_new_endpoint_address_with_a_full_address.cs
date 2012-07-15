using SystemDot.Messaging.Messages;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.endpoints
{
    [Subject("Endpoints")]
    public class when_initialising_a_new_endpoint_address_with_a_full_address
    {
        const string Address = "TestChannel@TestServer";
        static EndpointAddress address;

        Because of = () => address = Address;

        It should_set_the_specified_address = () => address.Address.ShouldEqual(Address);

        It should_set_the_channel = () => address.Channel.ShouldEqual("TestChannel");
        
        It should_set_the_specified_server_name = () => address.ServerName.ShouldEqual("TestServer");
    }
}