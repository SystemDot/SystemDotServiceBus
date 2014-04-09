using System;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply_over_http
{
    [Subject(SpecificationGroup.Description)]
    public class when_a_server_error_occurs_when_sending_a_request_with_an_error_action_specified : WithHttpServerConfigurationSubject
    {
        const long Message = 1;
        static Exception expectedException;
        static Exception exception;

        Establish context = () =>
        {
            expectedException = new Exception();
            WebRequestor.ThrowExceptionOnPut(expectedException);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer("SenderServer")
                .OpenDirectChannel("SenderChannel")
                    .ForRequestReplySendingTo("ReceiverChannel@ReceiverServer")
                .Initialise();
        };

        Because of = () => Bus.SendDirect(Message, e => exception = e);

        It should_run_the_server_error_action = () => exception.ShouldBeEquivalentTo(expectedException);
    }
}