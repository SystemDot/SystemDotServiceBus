namespace SystemDot.Specifications.ioc
{
    public class AnotherTestComponent : IAnotherTestComponent
    {
        public IThirdTestComponent FirstParameter;
        public IAnotherTestComponent RepeatedComponent;

        public AnotherTestComponent(IThirdTestComponent firstParameter, IAnotherTestComponent repeatedComponent)
        {
            this.FirstParameter = firstParameter;
            this.RepeatedComponent = repeatedComponent;
        }
    }
}