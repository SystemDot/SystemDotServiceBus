namespace SystemDot.Messaging.Direct.Builders
{
    class LocalDirectChannelSchema
    {
        public IMessageProcessor<object, object> UnitOfWorkRunner { get; set; }
    }
}