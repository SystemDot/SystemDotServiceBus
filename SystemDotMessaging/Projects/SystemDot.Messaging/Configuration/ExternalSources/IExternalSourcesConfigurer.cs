namespace SystemDot.Messaging.Configuration.ExternalSources
{
    public interface IExternalSourcesConfigurer
    {
        void Configure(MessageServerConfiguration toConfigureAgainst);
    }
}