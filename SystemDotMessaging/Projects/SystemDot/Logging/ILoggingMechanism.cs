namespace SystemDot.Logging
{
    public interface ILoggingMechanism
    {
        bool ShowInfo { get; set; }

        void Info(string message);

        void Error(string message);
    }
}