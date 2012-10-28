namespace SystemDot.Messaging.Storage
{
    public enum PersistenceUseType
    {
        RequestSend,
        RequestReceive,
        ReplySend,
        ReplyReceive,
        PublisherSend,
        SubscriberSend,
        SubscriberReceive,
        SubscriberRequestSend,
        SubscriberRequestReceive
    }
}