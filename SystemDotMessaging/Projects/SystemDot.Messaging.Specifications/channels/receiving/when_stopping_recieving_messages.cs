using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Recieving;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.receiving
{
    public class when_stopping_recieving_messages
    {
        static MessageReciever reciever;
        static TestMessageListener listener;
        
        Establish context = () =>
        {
            listener = new TestMessageListener();
            reciever = new MessageReciever(new Pipe(), listener);
            reciever.StartRecieving();
        };

        Because of = () => reciever.StopRecieving();

        It should_stop_listening_for_messages = () => listener.IsRunning.ShouldBeFalse();
    }
}  