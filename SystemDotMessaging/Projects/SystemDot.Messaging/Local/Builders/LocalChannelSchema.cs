namespace SystemDot.Messaging.Local.Builders
{
    class LocalChannelSchema
    {
        public IMessageProcessor<object, object> UnitOfWorkRunner { get; set; }
    }
}