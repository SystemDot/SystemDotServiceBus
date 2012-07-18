using SystemDot.Messaging.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.endpoints
{
    [Subject("Endpoints")]
    public class when_building_a_new_endpoint_address_with_a_specified_channel : WithSubject<EndpointAddressBuilder>
    {
        const string TestChannelName = "TestChannel";
        const string TestServer = "TestServer";
        static EndpointAddress address;

        Establish context = () => Configure<IMachineIdentifier>(new MachineIdentifier());

        Because of = () => address = Subject.Build(TestChannelName, TestServer); 

        It should_set_the_specified_address = () => 
            address.Channel.ShouldEqual(TestChannelName + "@" + The<IMachineIdentifier>().GetMachineName());

        It should_set_the_specified_server_name = () => address.ServerName.ShouldEqual(TestServer);
    }
}