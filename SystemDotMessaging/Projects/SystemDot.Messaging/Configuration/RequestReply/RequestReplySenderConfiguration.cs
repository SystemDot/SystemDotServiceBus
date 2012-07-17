namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplySenderConfiguration : Configurer
    {
        public IBus Initialise()
        {
            return Resolve<IBus>();
        }
    }
}