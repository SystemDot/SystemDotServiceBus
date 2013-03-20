namespace System.IoC.Example.Types
{
    internal class CustomType : ICustomInterface
    {
        public string Say()
        {
            return "I am a registered. I am CustomType.";
        }
    }
}