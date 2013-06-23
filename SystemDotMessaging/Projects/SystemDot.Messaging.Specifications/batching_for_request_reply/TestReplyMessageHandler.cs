namespace SystemDot.Messaging.Specifications.batching_for_request_reply
{
    public class TestReplyMessageHandler<T>
    {
        public void Handle(T message)
        {
            Bus.Reply(message);
        }
    }
}