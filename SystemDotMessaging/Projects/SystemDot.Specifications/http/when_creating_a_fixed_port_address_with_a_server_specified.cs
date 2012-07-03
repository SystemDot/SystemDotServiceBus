using SystemDot.Http;
using Machine.Specifications;

namespace SystemDot.Specifications.http
{
    public class when_creating_a_fixed_port_address_with_a_server_specified
    {
        static FixedPortAddress address;
        static string server;
        
        Establish context = () =>
        {
            server = "test";
        };

        Because of = () => address = new FixedPortAddress(server);

        It should_provide_a_url_with_the_correct_address = () => address.Url.ShouldEqual("http://" + server + ":8090/");
    }
}