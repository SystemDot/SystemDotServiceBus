using SystemDot.Messaging.Messages;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.endpoints
{
    [Subject("Endpoints")]
    public class when_initialising_a_new_endpoint_address_with_only_a_local_address
    {
        const string ChannelName = "TestChannel";
        static EndpointAddress address;

        Because of = () => address = ChannelName;

        It should_set_the_specified_address = () => address.Address.ShouldEqual(ChannelName);

        It should_set_the_channel = () => address.Channel.ShouldEqual(ChannelName);

        It should_not_set_the_server_name = () => address.ServerName.ShouldBeEmpty();
    }
}