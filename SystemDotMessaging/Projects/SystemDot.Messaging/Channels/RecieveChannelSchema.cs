namespace SystemDot.Messaging.Channels
{
    public class RecieveChannelSchema
    {
        public EndpointAddress Address { get; set; }
        public bool IsSequenced { get; set; }
    }
}