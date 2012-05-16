using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Sending;
using SystemDot.Messaging.Specifications.channels.publishing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.http_sending
{
    public class when_sending_a_message_over_http
    {
        static Pipe pipe;
        static string message;
        static TestMessageWebRequestor requestor;
         
        Establish context = () =>
        {
            message = "message";

            pipe = new Pipe();
            requestor = new TestMessageWebRequestor();
            new MessageSender(pipe, requestor);
        };

        Because of = () => pipe.Publish(message);

        It should_send_the_message_via_http_request = () =>  
            requestor.SentMessages.ShouldContain(message);
    }
}  