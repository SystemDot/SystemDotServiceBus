namespace SystemDot.Specifications.ioc
{
    public class TestComponent : ITestComponent
    {
        public string ConstructorArgument1 { get; private set; }

        public int ConstructorArgument2 { get; private set; }

        public TestComponent() {}

        public TestComponent(string constructorArgument1)
        {
            ConstructorArgument1 = constructorArgument1;
        }

        public TestComponent(string constructorArgument1, int constructorArgument2)
        {
            ConstructorArgument1 = constructorArgument1;
            ConstructorArgument2 = constructorArgument2;
        }
    }
}