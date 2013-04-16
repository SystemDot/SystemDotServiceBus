namespace SystemDot.Messaging.Configuration.ExternalSources
{
    public interface IExternalSourcesConfigurer
    {
        void Configure(MessagingConfiguration toConfigureAgainst, MessageServerConfiguration serverToConfigureAgainst);
    }
}