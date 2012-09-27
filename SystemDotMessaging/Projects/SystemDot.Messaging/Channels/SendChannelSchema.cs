namespace SystemDot.Messaging.Channels
{
    public class SendChannelSchema
    {
        public EndpointAddress FromAddress { get; set; }

        public bool IsPersistent { get; set; }
    }
}