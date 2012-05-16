using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Recieving;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.receiving
{
    public class when_receiving_a_message_into_a_pipe_using_http
    {
        static Pipe pipe;
        static string message;
        static string recievedMessage;
        static MessageReciever reciever;
         
        Establish context = () =>
        {
            message = "message";
            pipe = new Pipe();
            pipe.MessagePublished += o => recievedMessage = (string)o;

            var listener = new TestMessageListener(message);
            reciever = new MessageReciever(pipe, listener);
        };

        Because of = () => reciever.StartRecieving();

        It should_recieve_the_message_into_the_pipe = () => recievedMessage.ShouldBeTheSameAs(message);
    }
}  