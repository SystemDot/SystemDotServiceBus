using System;
using SystemDot.Http;
using Machine.Specifications;

namespace SystemDot.Specifications.http
{
    public class when_creating_a_fixed_port_address_without_a_server_specified
    {
        static FixedPortAddress address;

        Because of = () => address = new FixedPortAddress();

        It should_provide_a_url_with_the_correct_address = () => address.Url.ShouldEqual("http://" + Environment.MachineName + ":8090/");
    }
}