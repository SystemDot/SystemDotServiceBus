namespace SystemDot.Messaging.Addressing
{
    public class NullMessageServer : MessageServer
    {
        public override string ToString()
        {
            return "{NoServer}";
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}