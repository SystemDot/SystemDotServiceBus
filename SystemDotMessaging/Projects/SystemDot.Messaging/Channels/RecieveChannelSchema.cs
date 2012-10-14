namespace SystemDot.Messaging.Channels
{
    public class RecieveChannelSchema
    {
        public EndpointAddress Address { get; set; }
        public EndpointAddress ToAddress { get; set; }
        public bool IsSequenced { get; set; }
    }
}