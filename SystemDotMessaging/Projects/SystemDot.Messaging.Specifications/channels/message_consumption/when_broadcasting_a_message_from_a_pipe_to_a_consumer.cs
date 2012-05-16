using SystemDot.Messaging.Channels;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.message_consumption
{
    public class when_broadcasting_a_message_from_a_pipe_to_a_consumer
    {
        static Pipe pipe;
        static TestConsumer<string> consumer;

        Establish context = () =>
        {
            pipe = new Pipe();
            var broadcaster = new ConsumerMessageBroadcaster(pipe);
 
            consumer = new TestConsumer<string>();
            broadcaster.RegisterConsumer(consumer);
        };

        Because of = () => pipe.Publish("Test");
        
        It should_consume_the_message_in_the_consumer = () => consumer.ConsumedMessage.ShouldEqual("Test");
    }
}