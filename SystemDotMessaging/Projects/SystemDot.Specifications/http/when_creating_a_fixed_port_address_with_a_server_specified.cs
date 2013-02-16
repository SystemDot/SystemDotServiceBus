using SystemDot.Http;
using Machine.Specifications;

namespace SystemDot.Specifications.http
{
    public class when_creating_a_fixed_port_address_with_a_server_specified
    {
        const string Server = "test";
        const string Instance = "Instance";
        
        static FixedPortAddress address;
         
        Because of = () => address = new FixedPortAddress(Server, Instance);

        It should_provide_a_url_with_the_correct_address = () => 
            address.Url.ShouldEqual("http://" + Server + "/" + Instance + ":8090/");
    }
}