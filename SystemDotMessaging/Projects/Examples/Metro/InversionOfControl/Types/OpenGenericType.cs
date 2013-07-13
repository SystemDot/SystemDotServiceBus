namespace InversionOfControl.Types
{
    public class OpenGenericType<T>
    {
        public string Say()
        {
            return "I should not be registered. I am OpenGenericType<T>.";
        }
    }
}